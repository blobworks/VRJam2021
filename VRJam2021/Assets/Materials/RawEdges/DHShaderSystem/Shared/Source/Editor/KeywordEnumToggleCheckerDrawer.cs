using UnityEngine;
using UnityEditor;

namespace DHShaderSystem
{
    public class CheckerToggleKeywordEnumDrawer : MaterialPropertyDrawer
    {
        protected string _keyword;
        private readonly GUIContent[] keywords;

        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1) : this(keyword, new[] { kw1 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2) : this(keyword, new[] { kw1, kw2 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2, string kw3) : this(keyword, new[] { kw1, kw2, kw3 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2, string kw3, string kw4) : this(keyword, new[] { kw1, kw2, kw3, kw4 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2, string kw3, string kw4, string kw5) : this(keyword, new[] { kw1, kw2, kw3, kw4, kw5 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2, string kw3, string kw4, string kw5, string kw6) : this(keyword, new[] { kw1, kw2, kw3, kw4, kw5, kw6 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7) : this(keyword, new[] { kw1, kw2, kw3, kw4, kw5, kw6, kw7 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7, string kw8) : this(keyword, new[] { kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7, string kw8, string kw9) : this(keyword, new[] { kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8, kw9 }) { }
        public CheckerToggleKeywordEnumDrawer(string keyword, params string[] keywords)
        {
            _keyword = keyword;

            this.keywords = new GUIContent[keywords.Length];

            for (int i = 0; i < keywords.Length; ++i)
                this.keywords[i] = new GUIContent(keywords[i]);
        }

        static bool IsPropertyTypeSuitable(MaterialProperty prop)
        {
            return prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range;
        }

        void SetKeyword(MaterialProperty prop, int index)
        {
            for (int i = 0; i < keywords.Length; ++i)
            {
                string keyword = GetKeywordName(prop.name, keywords[i].text);

                foreach (Material material in prop.targets)
                {
                    if (index == i)
                        material.EnableKeyword(keyword);
                    else
                        material.DisableKeyword(keyword);
                }
            }
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (IsKeywordEnabled(editor) == false)
                return 0;

            if (!IsPropertyTypeSuitable(prop))
                return EditorGUIUtility.singleLineHeight * 2.5f;

            return base.GetPropertyHeight(prop, label, editor);
        }

        public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.LabelField(position, "KeywordEnum used on a non-float property: ", EditorStyles.helpBox);
                return;
            }

            if (IsKeywordEnabled(editor) == false)
                return;

            EditorGUI.BeginChangeCheck();

            EditorGUI.showMixedValue = prop.hasMixedValue;
            var value = (int)prop.floatValue;
            value = EditorGUI.Popup(position, label, value, keywords);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                prop.floatValue = value;
                SetKeyword(prop, value);
            }
        }

        public override void Apply(MaterialProperty prop)
        {
            base.Apply(prop);
            if (!IsPropertyTypeSuitable(prop))
                return;

            if (prop.hasMixedValue)
                return;

            SetKeyword(prop, (int)prop.floatValue);
        }

        // Final keyword name: property name + "_" + display name. Uppercased,
        // and spaces replaced with underscores.
        private static string GetKeywordName(string propName, string name)
        {
            string n = propName + "_" + name;
            return n.Replace(' ', '_').ToUpperInvariant();
        }

        private bool IsKeywordEnabled(MaterialEditor editor)
        {
            Material material = editor.target as Material;

            if (material != null)
                return material.IsKeywordEnabled(_keyword);

            return false;
        }
    }
}