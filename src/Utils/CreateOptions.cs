using UnityEngine;
using PeglinUI.SettingsMenu.Options;
using TMPro;
using UnityEngine.UI;

namespace PegTheStreamer {
    public static partial class Utils {

        public static Transform FindContents() {
            return GameObject.Find("PauseCanvas").transform.Find("NewOptionsPanel/BasePanel/GeneralSettings/ScrollView/OptionsSubpanel/Viewport/Contents");
        }

        public static void SetOptionDescription(GameObject option, string description) {
            (option.transform.Find("Text (TMP)").GetComponent(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI).text = description;

            Object.Destroy(option.transform.Find("Text (TMP)").GetComponent(typeof(I2.Loc.Localize)));
        }

        public static CarouselTextItem[] textsFromStrings(string[] strings) {
            var ans = new CarouselTextItem[strings.Length];
            for (int i = 0; i < strings.Length; ++i) {
                ans[i].item = strings[i];
                ans[i].isTranslatable = false;
            }
            return ans;
        }

        public static void SetCarouselOptionsText(CarouselOptions option, string[] strings) {
            option.textItems = textsFromStrings(strings);
        }

        public static void SetOptionColor(GameObject option, Color color) {
            (option.GetComponent(typeof(UnityEngine.UI.Image)) as UnityEngine.UI.Image).color = color;
        }

        private static GameObject CopyOption(string optionToCopy, string newDescription, Color newColor) {
            Transform contents = FindContents();
            GameObject option = GameObject.Instantiate(contents.Find(optionToCopy).gameObject, contents);
            SetOptionDescription(option, newDescription);
            SetOptionColor(option, newColor);
            return option;
        }

        public static CarouselOptionsBoolean CreateBinaryOption(string description, Color color) {
            return CopyOption("VSyncSelection", description, color).GetComponent(typeof(CarouselOptionsBoolean)) as CarouselOptionsBoolean;
        }

        public static CarouselOptionsInt CreateIntOption(string description, Color color, string[] optionsDescriptions) {
            CarouselOptionsInt option = CopyOption("VignetteControl", description, color).GetComponent(typeof(CarouselOptionsInt)) as CarouselOptionsInt;
            SetCarouselOptionsText(option, optionsDescriptions);
            return option;
        }

        // it just works section starts here

        private static void DeleteCarousel(GameObject option) {
            // code smell
            GameObject.DestroyImmediate(option.transform.Find("ArrowSelection").gameObject);
            GameObject.DestroyImmediate(option.transform.Find("Carousel").gameObject);
            GameObject.DestroyImmediate(option.GetComponent(typeof(CarouselOptionsBoolean)));
            GameObject.DestroyImmediate(option.GetComponent(typeof(Selectable)));
        }

        private static GameObject AddViewport(GameObject option) {
            GameObject viewport = new GameObject("Text Area", typeof(RectTransform));
            RectTransform transform = viewport.GetComponent(typeof(RectTransform)) as RectTransform;
            viewport.AddComponent<RectMask2D>().padding = Vector4.zero;
            transform.SetParent(option.transform, false);
            transform.localScale = Vector3.one;
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.sizeDelta = Vector2.zero;
            transform.pivot = new Vector2(0, 0.5f);
            transform.offsetMin = new Vector2(570, 0);  // <- two magic values that matter, just kill me, why was it so hard to align this thing
            transform.offsetMax = new Vector2(-110, 0); // <-
            return viewport;
        }

        public static TMP_InputField CreateInputFieldOption(string description, string placeholder, Color color, Color textColor) {
            GameObject option = CopyOption("VSyncSelection", description, color);
            DeleteCarousel(option);
            GameObject viewport = AddViewport(option);

            TextMeshProUGUI inputText = CreateText(viewport.transform, "", new Vector3(120, 0, 0));
            inputText.color = textColor;
            inputText.verticalAlignment = VerticalAlignmentOptions.Capline;
            inputText.enableWordWrapping = false;

            TextMeshProUGUI placeholderText = CreateText(viewport.transform, placeholder, new Vector3(120, 0, 0));
            textColor.a *= 0.5f;
            placeholderText.color = textColor;
            placeholderText.verticalAlignment = VerticalAlignmentOptions.Capline;
            placeholderText.enableWordWrapping = false;

            TMP_InputField inputField = option.AddComponent(typeof(TMP_InputField)) as TMP_InputField;
            inputField.textViewport = viewport.transform as RectTransform;
            inputField.textComponent = inputText;
            inputField.placeholder = placeholderText;

            return inputField;
        }

        // it just works section ends here
    }
}
