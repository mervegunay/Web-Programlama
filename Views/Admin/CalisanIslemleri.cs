@model IEnumerable<Kuafor1.Models.Calisan>

@{
    ViewData["Title"] = "Çalışan İşlemleri";
}

< h2 class= "text-center" > Çalışan İşlemleri </ h2 >

< div class= "container mt-4" >

    < !--Başarı / Başarısız Mesajları-- >
    @if(TempData["Mesaj"] != null)
    {
        < div class= "alert alert-info" >
            @TempData["Mesaj"]
        </ div >
    }

    < !--Çalışan Ekleme Formu -->
    <div class= "card mb-4" >
        < div class= "card-header bg-primary text-white" >
            < h5 class= "mb-0" > Yeni Çalışan Ekle</h5>
        </div>
        <div class= "card-body" >
            < form method = "post" asp - controller = "Admin" asp - action = "CalisanEkle" >
                < div class= "form-group mb-3" >
                    < label for= "calisanAd" > Çalışan Adı:</ label >
                    < input type = "text" class= "form-control" id = "calisanAd" name = "calisanAd" required />
                </ div >
                < div class= "form-group mb-3" >
                    < label for= "calisanUygunluk" > Çalışan Uygunluk:</ label >
                    < input type = "text" class= "form-control" id = "calisanUygunluk" name = "calisanUygunluk" required />
                </ div >
                < button type = "submit" class= "btn btn-primary" > Ekle </ button >
            </ form >
        </ div >
    </ div >

    < !--Çalışan Listeleme-- >
    < div class= "card" >
        < div class= "card-header bg-success text-white" >
            < h5 class= "mb-0" > Mevcut Çalışanlar </ h5 >
        </ div >
        < div class= "card-body" >
            < table class= "table table-bordered" >
                < thead >
                    < tr >
                        < th > Ad </ th >
                        < th > Uygunluk </ th >
                        < th > İşlemler </ th >
                    </ tr >
                </ thead >
                < tbody >
                    @foreach(var calisan in Model)
                    {
                        < tr >
                            < td > @calisan.CalisanAd </ td >
                            < td > @calisan.CalisanUygunluk </ td >
                            < td >
                                < a asp - controller = "Admin" asp - action = "CalisanSil" asp - route - id = "@calisan.CalisanId" class= "btn btn-danger btn-sm" > Sil </ a >
                            </ td >
                        </ tr >
                    }
                </ tbody >
            </ table >
        </ div >
    </ div >

    < !--İşlemler Sayfasına Geri Dön Butonu -->
    <div class= "text-center mt-4" >
        < a asp - controller = "Admin" asp - action = "Islemler" class= "btn btn-secondary btn-lg" > İşlemler Sayfasına Dön</a>
    </div>

</div>

