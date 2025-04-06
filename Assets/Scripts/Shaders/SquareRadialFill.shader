Shader "AdultLink/RadialProgressBar/SimpleRadialProgressBarSquare"
{
    Properties
    {
        _Radius("Radius", Range( 0 , 1)) = 0.25
        _Arcrange("Arc range", Range( 0 , 360)) = 360
        _Fillpercentage("Fill percentage", Range( 0 , 1)) = 0.25
        _Globalopacity("Global opacity", Range( 0 , 1)) = 1
        [HDR]_Barmincolor("Bar min color", Color) = (1,0,0,1)
        [HDR]_Barmaxcolor("Bar max color", Color) = (0,1,0.08965516,1)
        _Rotation("Rotation", Range( 0 , 360)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back

            CGPROGRAM
            #pragma surface surf Standard alpha:fade

            #include "UnityCG.cginc"

            struct Input
            {
                float2 uv_texcoord;
            };

            uniform float _Radius;
            uniform float _Fillpercentage;
            uniform float _Arcrange;
            uniform float _Rotation;
            uniform float4 _Barmincolor;
            uniform float4 _Barmaxcolor;
            uniform float _Globalopacity;

            void surf(Input i, inout SurfaceOutputStandard o)
            {
                // Convert UV coordinates to range [-1, 1]
                float2 uv = i.uv_texcoord * 2.0 - 1.0;

                // Calculate length and angle from the center
                float len = length(uv);
                float angle = atan2(uv.y, uv.x) * (180.0 / 3.14159265359); // Convert to degrees

                // Apply rotation to angle
                angle += _Rotation;
                if (angle < 0) angle += 360.0;
                if (angle > 360.0) angle -= 360.0;

                // Calculate fill threshold based on arc range and fill percentage
                float fillThreshold = _Fillpercentage * _Arcrange;

                // Check if the current pixel is within the fill range
                bool isFilled = (angle <= fillThreshold && len <= _Radius);

                // Interpolate between min and max colors based on fill percentage
                float4 barColor = lerp(_Barmincolor, _Barmaxcolor, _Fillpercentage);

                // Set the color and opacity for the filled area
                if (isFilled)
                {
                    o.Albedo = barColor.rgb;
                    o.Emission = barColor.rgb;
                    o.Alpha = _Globalopacity;
                }
                else
                {
                    o.Albedo = float3(0, 0, 0); // Transparent for unfilled area
                    o.Alpha = 0;
                }
            }

            ENDCG
        }
    }

    Fallback "Diffuse"
}
