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