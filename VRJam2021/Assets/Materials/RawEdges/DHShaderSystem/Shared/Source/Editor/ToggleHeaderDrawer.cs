using UnityEngine;
using UnityEditor;

namespace DHShaderSystem
{
    public class ToggleHeaderDrawer : MaterialPropertyDrawer
    {
        protected readonly float height;
        protected readonly string header;
        protected readonly string keyword;

        public ToggleHeaderDrawer(string header_keyword)
        {
            this.keyword = header_keyword.ToUpper();
            this.header = header_keyword;
        }

        public ToggleHeaderDrawer(string header, string keyword)
        {
            this.keyword = keyword.ToUpper();
            this.header = header;
        }

        public ToggleHeaderDrawer(string header_keyword, float height)
        {
            this.height = height;
            this.keyword = header_keyword.ToUpper();
            this.header = header_keyword;
        }

        public ToggleHeaderDrawer(string header, string keyword, float height)
        {
            this.height = height;
            this.keyword = keyword.ToUpper();
            this.header = header;
        }

        static bool IsPropertyTypeSuitable(MaterialProperty prop)
        {
            return prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                return 18 * 2.5f;
            }
            return base.GetPropertyHeight(prop, label, editor);
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            position = EditorGUI.IndentedRect(position);

            bool value = (Mathf.Abs(prop.floatValue) > 0.001f);

            if (height > 0)
            {
                if (value)
                    GUI.Box(new Rect(position.x - 4, position.y - 3, position.width + 8, height), "");
                else
                    GUI.Box(new Rect(position.x - 4, position.y - 3, position.width + 8, 22), "");
            }

            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.LabelField(position, "Toggle used on a non-float property: " + prop.name, EditorStyles.helpBox);
                return;
            }

            EditorGUI.BeginChangeCheck();

            EditorGUI.showMixedValue = prop.hasMixedValue;
            value = EditorGUI.Toggle(position, value);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                prop.floatValue = value ? 1.0f : 0.0f;
                SetKeyword(prop, value);
            }

            position.x += 18;
            GUI.Label(position, header, EditorStyles.boldLabel);
        }

        public override void Apply(MaterialProperty prop)
        {
            base.Apply(prop);
            if (!IsPropertyTypeSuitable(prop))
                return;

            if (prop.hasMixedValue)
                return;

            SetKeyword(prop, (Mathf.Abs(prop.floatValue) > 0.001f));
        }

        protected void SetKeyword(MaterialProperty prop, bool on)
        {
            foreach (Material material in prop.targets)
            {
                if (on)
                    material.EnableKeyword(keyword);
                else
                    material.DisableKeyword(keyword);
            }
        }
    }
}