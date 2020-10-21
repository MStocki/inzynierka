using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android;
using Plugin.Media;
using Android.Graphics;

namespace AplikacjaRozpoznajacaOpakowaniaLekow
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button cameraButton; //definiujemy przycisk i imageview z activity main 
        ImageView thisImageView;

        //tablica uprawnien 
        readonly string[] permissions =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            cameraButton = (Button)FindViewById(Resource.Id.cameraButton);//szukamy przycisku po id, podanym w activity main 
            thisImageView = (ImageView)FindViewById(Resource.Id.thisImageView);

            cameraButton.Click += CameraButton_Click; //event po kliknieciu

            RequestPermissions(permissions,0);
        }

        private void CameraButton_Click(object sender, System.EventArgs e)
        {
            TakePhoto();
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();//inicjalizujemy pakiet xam.media 

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,//wielkosc zdjecia
                CompressionQuality = 30,//stopien kompresji w % 
                Name="myimage.jpg",
                Directory="sample"
            });

            if (file == null) return;

            //konwertujemy na tablice bitow i wynik zapisujemy w bitmapie, dodajac do imageview
            byte[] imgArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imgArray,0,imgArray.Length);
            thisImageView.SetImageBitmap(bitmap);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
    }
}