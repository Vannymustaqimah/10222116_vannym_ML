# Dataset Deteksi Berita Hoaks Indonesia

## Deskripsi

Dataset ini dikembangkan untuk mendukung penelitian dan eksperimen dalam bidang Natural Language Processing (NLP), khususnya dalam deteksi berita hoaks dalam bahasa Indonesia. Data diperoleh melalui scraping dari situs berita online Indonesia yang kredibel, yaitu:

- [TurnBackHoax.id](https://turnbackhoax.id)
- [CNN Indonesia](https://www.cnnindonesia.com)
- [Kompas](https://www.kompas.com)
- [Detik](https://www.detik.com)

Dataset dibagi menjadi tiga folder utama:

- `RAW`: Berisi metadata hasil scraping (judul, tanggal, URL, Isi Berita).
- `Cleaned`: Berisi data yang telah dibersihkan (tanpa isi artikel penuh).
- `Summary`: Berisi ekstraksi fitur seperti kategori, label hoaks/non-hoaks, dan analisis sentimen.

## Detail Dataset

- Bahasa: Bahasa Indonesia
- Format: `.csv` dan `.xlsx`
- Jumlah entri:
  - TurnBackHoax.id: 12.648
  - CNN Indonesia: 4.216
  - Kompas: 4.216
  - Detik: 4.216

## Tujuan Penggunaan

- Riset akademik
- Analisis tren penyebaran hoaks
- Pelatihan model klasifikasi teks
- Pengembangan sistem pendeteksi berita palsu

## Lisensi dan Penggunaan

Lisensi: **Custom — Research and Education Use Only**

> Seluruh hak cipta atas konten berita berada pada masing-masing pemilik situs. Dataset ini disediakan hanya untuk keperluan riset dan pendidikan. Penggunaan ulang atau distribusi ulang konten ini untuk tujuan komersial tidak diizinkan.

Jika Anda adalah pemilik konten dan ingin meminta penghapusan, silakan hubungi penyusun dataset melalui email yang tersedia di profil.

## Atribusi

Jika Anda menggunakan dataset ini, mohon cantumkan atribusi ke:

- Sumber berita asli (CNN Indonesia, Kompas, Detik, TurnBackHoax.id)
- Pembuat dataset: Wersbo
