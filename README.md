# DietCok – ASP.NET Core Kalori Takip Uygulaması

DietCok, kullanıcıların günlük **kalori ve makro besin tüketimlerini takip edebilmesi**, kişisel hedeflerini belirleyebilmesi ve beslenme durumunu günlük / aylık olarak görüntüleyebilmesi amacıyla geliştirdiğim bir web uygulamasıdır.

Bu sürüm, **ASP.NET Core MVC ve C#** kullanılarak geliştirilmiş olup Entity Framework Core ile veri yönetimi, SQLite ile kalıcı veri saklama ve REST API desteği sunmaktadır.

## Temel Özellikler

- Kullanıcı kayıt, giriş ve güvenli çıkış işlemleri
- Cookie Authentication ile oturum yönetimi
- BCrypt ile şifrelerin hash'lenmesi
- Günlük kalori, protein, karbonhidrat ve yağ hedeflerinin belirlenmesi
- Kahvaltı, öğle yemeği, akşam yemeği ve ara öğün kategorilerinde tüketim kaydı
- Besinlerin aranarak öğünlere eklenmesi
- Günlük toplam kalori ve makro değerlerinin otomatik hesaplanması
- Kullanıcı hedefleri ile günlük tüketimlerin karşılaştırılması
- Aylık takvim üzerinden beslenme geçmişinin görüntülenmesi
- Hedefin altında ve üzerinde kalan günlerin renk kodlarıyla gösterilmesi
- Profil üzerinden günlük hedeflerin güncellenmesi
- Besin kayıtlarının eklenmesi ve silinmesi
- RESTful API üzerinden besin verilerine erişim
- Swagger / OpenAPI ile API dokümantasyonu ve test arayüzü

## Teknolojiler

`C#` `ASP.NET Core MVC` `.NET 8` `Entity Framework Core 8` `SQLite` `Razor Views` `LINQ` `Dependency Injection` `Cookie Authentication` `BCrypt` `REST API` `Swagger / OpenAPI` `Postman` `HTML` `CSS` `JavaScript` `jQuery` `Select2`

## Mimari Yapı

Proje ASP.NET Core'un **Model–View–Controller (MVC)** yapısını kullanmaktadır.

```text
Controllers/
├── AccountController.cs
├── DashboardController.cs
├── ProfileController.cs
└── Api/
    └── FoodApiController.cs

Data/
├── CalorieTrackerContext.cs
└── DbInitializer.cs

Models/
├── AccountViewModels.cs
├── Consumption.cs
├── Food.cs
└── User.cs

Views/
├── Account/
├── Dashboard/
├── Profile/
└── Shared/

wwwroot/
├── css/
└── img/
```

Uygulamada ayrı bir Service / Repository katmanı oluşturulmamıştır. İş mantığı ilgili controller sınıflarında yürütülürken, veritabanı işlemleri Dependency Injection ile controller'lara aktarılan `CalorieTrackerContext` üzerinden Entity Framework Core kullanılarak gerçekleştirilmektedir.

## Veri Modeli

Uygulamanın temel veri modeli üç ana entity üzerine kuruludur:

- `User` — kullanıcı hesabı ve günlük kalori / makro hedefleri
- `Food` — besin bilgileri ve besin değerleri
- `Consumption` — kullanıcının belirli bir öğünde gerçekleştirdiği tüketim kaydı

`User` ile `Consumption` ve `Food` ile `Consumption` arasında bire-çok ilişkiler bulunmaktadır.

## Dashboard ve Takvim

Dashboard, kullanıcının aylık beslenme durumunu gün bazında görüntüler.

Her gün için:

- toplam kalori,
- protein,
- karbonhidrat,
- yağ

değerleri hesaplanır ve kullanıcının profilinde tanımlanan hedeflerle karşılaştırılır.

Kalori hedefinin altında kalan günler **yeşil**, hedefin üzerine çıkan günler ise **kırmızı** olarak gösterilir.

## REST API

Projede `FoodApiController` üzerinden besin verilerine erişim sağlayan RESTful API uç noktaları bulunmaktadır.

API servislerinin dokümantasyonu ve interaktif olarak test edilebilmesi için **Swagger / OpenAPI** entegrasyonu kullanılmıştır. API testlerinde ayrıca **Postman** kullanılmıştır.

## Güvenlik

Kullanıcı oturumları **Cookie Authentication** ile yönetilmektedir.

Parolalar veritabanında düz metin olarak tutulmaz; **BCrypt.Net-Next** kullanılarak hash'lenir.

## Projeyi Çalıştırma

Projeyi çalıştırmak için **.NET 8 SDK** gereklidir.

Repository'yi klonladıktan sonra proje klasöründe:

```bash
dotnet restore
dotnet run
```

komutları çalıştırılabilir.

Uygulama ilk çalıştırıldığında SQLite veritabanı proje dizininde oluşturulur ve başlangıç verileri `DbInitializer` üzerinden hazırlanır.

Swagger arayüzüne uygulama çalışırken `/swagger` rotası üzerinden erişilebilir.

## Proje Raporu

Projenin mimarisi, kullanılan teknolojiler, UML sınıf diyagramı, uygulama ekranları, karşılaşılan hatalar ve çözümleri ayrıntılı olarak proje raporunda yer almaktadır.

📄 [`DIETCOK_ASPNET_FINAL_REPORT.pdf`](./DIETCOK_ASPNET_FINAL_REPORT.pdf)

## Diğer DietCok Sürümü

Bu repository, DietCok projesinin **ASP.NET Core / C# sürümüdür**.

Aynı uygulama fikrinin Java ve Spring Boot teknolojileriyle geliştirilen, yazılım testi ve kalite güvencesi ağırlıklı ayrı bir sürümü de GitHub profilimde bulunmaktadır.
