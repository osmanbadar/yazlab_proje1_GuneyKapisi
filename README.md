# Güney Kapısı

**Ekip:** Zeynep Uğurlu, Osman Badar
**Ders:** Yazılım Geliştirme Laboratuvarı-I, Proje 1
**Tarih:** 6 Kasım 2026

## 1. Giriş

Bu projede Unity ile 3D bir macera oyunu yaptık. Oyunda
birbirinden bağımsız iki yapay zekâ bileşeni var: oyuncuya
eşlik eden Pip ve kapıyı koruyan Gorvan. Pip yolunu NavMesh
ile buluyor, kararlarını durum makinesiyle veriyor. Gorvan
ise Ollama üzerinde yerelde çalışan bir dil modeliyle
konuşuyor.

Köyde salgın var, ilaçlar ise Güney Kapısı'nın arkasındaki
depoda. Oyuncu oraya ulaşmaya çalışıyor:

1. **Yıkık köprü:** Önümüzde bir uçurum ve kalkık bir köprü
   var. Köprüyü indiren şalter, duvarlarla kapalı bir odada.
   Odaya giden alçak tünele oyuncu giremiyor, sadece Pip
   sığıyor. Oyuncu E'ye basınca Pip şaltere gidiyor.
2. **Kamp:** Köprüden sonra kamp alanı geliyor. Burada
   muhtarın mektubu bulunuyor. Mektubu almak zorunlu değil.
3. **Güney Kapısı:** Oyuncu Gorvan'la serbest metinle
   konuşuyor. Gorvan ikna olursa kapı açılıyor ve oyun
   kazanılıyor. Gorvan saldırırsa oyun kaybediliyor.

## 2. Otonom Kahraman: Pip

Pip'e rota vermiyoruz, sadece hedef veriyoruz. Şaltere giden
yolu `NavMeshAgent` kendisi hesaplıyor. Köprü indiğinde
NavMesh yeniden oluşturuluyor, böylece Pip karşıya da geçip
oyuncuyu takip edebiliyor.

Karar mantığı `CompanionAgent` içindeki durum makinesinde.
Her durum `ICompanionState` arayüzünü uygulayan ayrı bir
sınıf olduğu için yeni bir durum eklerken diğerlerini
değiştirmemiz gerekmiyor.

```mermaid
stateDiagram-v2
    [*] --> Follow: Oyuncu atanmışsa
    [*] --> Idle: Oyuncu atanmamışsa
    Idle --> MoveToTarget: E emri
    Follow --> MoveToTarget: E emri
    MoveToTarget --> Wait: Önde engel var
    Wait --> MoveToTarget: Yol 0,3 sn açık kaldı
    Wait --> Failed: 10 sn geçti
    MoveToTarget --> Interact: Hedefe ulaştı
    MoveToTarget --> Failed: Yol yok veya 4 sn ilerleyemedi
    Failed --> MoveToTarget: Deneme hakkı var
    Failed --> Return: 3 tekrar da başarısız
    Interact --> Return: Şalter indirildi
    Return --> Follow: Oyuncuya ulaştı
```

| Durum | Pip ne yapıyor? |
|---|---|
| Idle | Bekliyor. |
| Follow | Oyuncuya 2,5 birimden uzaksa onu takip ediyor. |
| MoveToTarget | Şaltere gidiyor, yolu ve ilerlemesini kontrol ediyor. |
| Wait | Önünde engel varsa durup yolun açılmasını bekliyor. |
| Interact | Şalteri indiriyor. |
| Failed | 1 sn bekleyip yolu yeniden deniyor. En fazla 3 tekrar yapıyor. |
| Return | Oyuncunun yanına dönüyor. |

### Sensör

Tünelde yolu kesip açan hareketli bloklar var. Pip bunları
`RaycastSensor` ile algılıyor. Sensör, Pip'in önündeki 90
derecelik alana 7 ışın gönderiyor ve her ışın için mesafe ile
açıyı kaydediyor. Işınların menzili 4 birim. Önündeki ±35
derecelik alanda 1,5 birimden yakın bir engel varsa Pip
`Wait` durumuna geçiyor.

```mermaid
flowchart LR
    A[RaycastSensor<br/>7 ışın] --> B{1,5 birimden yakın<br/>engel var mı?}
    B -- Evet --> C[Wait]
    B -- Hayır --> D[MoveToTarget]
    C -- Yol açıldı --> D