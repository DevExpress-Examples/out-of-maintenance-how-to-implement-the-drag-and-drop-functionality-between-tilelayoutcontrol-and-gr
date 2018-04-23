using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.ComponentModel;

namespace DXSample
{
    public static class DataHelper
    {
        public static ObservableCollection<ExampleObject> GetData1()
        {
            ObservableCollection<ExampleObject> collection = new ObservableCollection<ExampleObject>()
            {
                new ExampleObject{Name = "Basket", ImageUri ="Images/Basket.png"},
                new ExampleObject{Name = "Check", ImageUri ="Images/Check.png"},
                new ExampleObject{Name = "Customer", ImageUri ="Images/Customer.png"},
            };
            return collection;
        }
        public static ObservableCollection<ExampleObject> GetData2()
        {
            ObservableCollection<ExampleObject> collection = new ObservableCollection<ExampleObject>()
            {
                new ExampleObject{Name = "Folder", ImageUri ="Images/Folder.png"},
                new ExampleObject{Name = "Key", ImageUri ="Images/Key.png"},
                new ExampleObject{Name = "Home", ImageUri ="Images/Home.png"},
            };
            return collection;
        }
    }
    public class ExampleObject : DevExpress.Xpf.Mvvm.BindableBase {
        // Fields...
        private string _ImageUri;
        private string _Name;

        public string Name {
            get { return _Name; }
            set {
                SetProperty(ref _Name, value, () => Name);
            }
        }

        public string ImageUri {
            get { return _ImageUri; }
            set {
                SetProperty(ref _ImageUri, value, () => ImageUri);
                RaisePropertiesChanged("ImageSource");
            }
        }

        public ImageSource ImageSource {
            get {
                return string.IsNullOrEmpty(ImageUri) ? null : new BitmapImage(new Uri("/DXSample;component/" + ImageUri, UriKind.Relative));
            }
        }
    }
}
