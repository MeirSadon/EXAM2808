using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM2808___Exrice1___SliderWithButton
{
    public class ViewModel
    {
        public Double SliderWidth { get; set; }
        public Double SliderHeight { get; set; }
        public string MyText { get; set; }
        public DelegateCommand MyCommand { get; set; }

        public ViewModel()
        {
            SliderWidth = 70;
            SliderHeight = 30;
            MyText = "";
            var self = this;
            MyCommand = new DelegateCommand(() => {
                new Parameters(self).ShowDialog();
            });
        }
    }
}
