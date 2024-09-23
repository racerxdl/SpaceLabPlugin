using System;
using System.Collections.Generic;
using Torch;

namespace SpaceLab
{
    public class SpaceLabConfig : ViewModel
    {
        private string _OpenAIToken = "Automagico";
        private string _StringProperty = "root";
        private int _IntProperty = 0;
        private bool _BoolProperty = true;
        private string _baseUrl = "http://192.168.55.11:20000/";

        public string BaseURL { get => _baseUrl; set => SetValue(ref _baseUrl, value); }

        public string OpenAIToken { get => _OpenAIToken; set => SetValue(ref _OpenAIToken, value); }

        public string StringProperty { get => _StringProperty; set => SetValue(ref _StringProperty, value); }
        public int IntProperty { get => _IntProperty; set => SetValue(ref _IntProperty, value); }
        public bool BoolProperty { get => _BoolProperty; set => SetValue(ref _BoolProperty, value); }
    }
}
