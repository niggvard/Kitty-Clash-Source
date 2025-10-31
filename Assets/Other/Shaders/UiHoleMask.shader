Shader "UI/HoleMask"
{
    Properties
    {
        _Color("Outer Color", Color) = (0, 0, 0, 0.8)
        _HoleCenter("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleRadius("Hole Radius", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float2 _HoleCenter;
            float _HoleRadius;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;

                float2 normalizedUV = float2(i.uv.x, i.uv.y / aspect);

                float dist = distance(normalizedUV, _HoleCenter);

                if (dist < _HoleRadius)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}
