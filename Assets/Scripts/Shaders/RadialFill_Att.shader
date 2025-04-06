Shader "Custom/SquareRadialProgressBar"
{
    Properties
    {
        _Maintex ("Main Texture", 2D) = "white" { }
        _Barmincolor ("Minimum Bar Color", Color) = (.5, .5, .5, 1)
        _Barmaxcolor ("Maximum Bar Color", Color) = (1, 1, 1, 1)
        _Fillpercentage ("Fill Percentage", Range(0, 1)) = 0.5
        _Maintexopacity ("Main Texture Opacity", Range(0, 1)) = 1.0
        _Maintextiling ("Main Texture Tiling", Vector) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Fillpercentage;
            float4 _Barmincolor;
            float4 _Barmaxcolor;
            sampler2D _Maintex;
            float4 _Maintex_ST;
            float _Maintexopacity;  // Ensure _Maintexopacity is declared

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Adjust UV to map to a square shape (preserving radial behavior)
                float2 uv = v.uv;
                uv = (uv - 0.5) * 2.0; // Center the UV coordinates
                float angle = atan2(uv.y, uv.x); // Get the angle for the radial fill
                float uvLength = length(uv); // Get the distance from the center (renamed to avoid conflict)
                uv = (uv * 0.5) + 0.5; // Rescale back to 0-1 range
                o.uv = uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Radial fill calculation
                float angle = atan2(i.uv.y - 0.5, i.uv.x - 0.5);
                float radialFill = (angle + 3.14159) / (2 * 3.14159); // Normalize angle to [0, 1]
                radialFill = saturate(radialFill - (1.0 - _Fillpercentage)); // Apply the fill percentage

                // Color lerping between the min and max colors based on fill
                half4 fillColor = lerp(_Barmincolor, _Barmaxcolor, radialFill);

                // Sample the texture
                half4 texColor = tex2D(_Maintex, i.uv);

                // Combine the texture with the fill color and apply opacity
                return fillColor * texColor.a * _Maintexopacity; // Ensure a value is returned
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}
