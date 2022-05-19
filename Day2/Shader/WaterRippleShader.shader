// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Scale ("Scale", float) = 1
        _Speed ("Speed", float) = 1
        _Frequency ("Frequency", float) = 1

        [MaterialToggle] _SinWave ("SinWave", Float) = 0 
        [MaterialToggle] _DiagnalWave ("DiagnalWave", Float) = 0 
        [MaterialToggle] _CenterWave ("CenterWave", Float) = 0 
        [MaterialToggle] _DiagnalWave2 ("DiagnalWave2", Float) = 0 

        _WaveAmplitude1 ("WaveAmplitude1", float) = 0
        _WaveAmplitude2 ("WaveAmplitude2", float) = 0
        _WaveAmplitude3 ("WaveAmplitude3", float) = 0
        _WaveAmplitude4 ("WaveAmplitude4", float) = 0
        _WaveAmplitude5 ("WaveAmplitude5", float) = 0
        _WaveAmplitude6 ("WaveAmplitude6", float) = 0
        _WaveAmplitude7 ("WaveAmplitude7", float) = 0
        _WaveAmplitude8 ("WaveAmplitude8", float) = 0

        _OffsetX1 ("OffsetX1", float) = 0
        _OffsetX2 ("OffsetX2", float) = 0
        _OffsetX3 ("OffsetX3", float) = 0
        _OffsetX4 ("OffsetX4", float) = 0
        _OffsetX5 ("OffsetX5", float) = 0
        _OffsetX6 ("OffsetX6", float) = 0
        _OffsetX7 ("OffsetX7", float) = 0
        _OffsetX8 ("OffsetX8", float) = 0

        _Distance1 ("Distance1", float) = 0
        _Distance2 ("Distance2", float) = 0
        _Distance3 ("Distance3", float) = 0
        _Distance4 ("Distance4", float) = 0
        _Distance5 ("Distance5", float) = 0
        _Distance6 ("Distance6", float) = 0
        _Distance7 ("Distance7", float) = 0
        _Distance8 ("Distance8", float) = 0

        _xImpact1 ("xImpact1", float) = 0
        _xImpact2 ("xImpact2", float) = 0
        _xImpact3 ("xImpact3", float) = 0
        _xImpact4 ("xImpact4", float) = 0
        _xImpact5 ("xImpact5", float) = 0
        _xImpact6 ("xImpact6", float) = 0
        _xImpact7 ("xImpact7", float) = 0
        _xImpact8 ("xImpact8", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        float _Scale, _Speed, _Frequency;
        half4 _Color;
        // 衝突による新しい振幅
        float _WaveAmplitude1, _WaveAmplitude2, _WaveAmplitude3, _WaveAmplitude4, _WaveAmplitude5, _WaveAmplitude6, _WaveAmplitude7, _WaveAmplitude8;
        float _OffsetX1, _OffsetZ1, _OffsetX2, _OffsetZ2, _OffsetX3, _OffsetZ3, _OffsetX4, _OffsetZ4, _OffsetX5, _OffsetZ5, _OffsetX6, _OffsetZ6, _OffsetX7, _OffsetZ7, _OffsetX8, _OffsetZ8;
        float _Distance1, _Distance2, _Distance3, _Distance4, _Distance5, _Distance6, _Distance7, _Distance8;
        float _xImpact1, _zImpact1, _xImpact2, _zImpact2, _xImpact3, _zImpact3, _xImpact4, _zImpact4, _xImpact5, _zImpact5, _xImpact6, _zImpact6, _xImpact7, _zImpact7, _xImpact8, _zImpact8;
        bool _SinWave, _DiagnalWave, _CenterWave, _DiagnalWave2;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert (inout appdata_full v)
        {
            // サイン波追加
            if(_SinWave){
                half offsetvert = v.vertex.x;
                
                half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);

                v.vertex.y += value;
                v.normal.z -= value;
            }

            // 対角サイン波追加
            if(_DiagnalWave){
                half offsetvert = v.vertex.x + v.vertex.z;
                
                half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);

                v.vertex.y += value;
                v.normal.z -= value;
            }

            // 中心から外延へのサイン波追加
            if(_CenterWave){
                half offsetvert = ((v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z));
                
                half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);

                v.vertex.y += value;
                v.normal.z -= value;
            }

            half offsetvert = ((v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z));
            half offsetvert2 = v.vertex.x + v.vertex.z;          
            
            half value0 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert2);
            
            half value1 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX1) + (v.vertex.z * _OffsetZ1));
            half value2 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX2) + (v.vertex.z * _OffsetZ2));
            half value3 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX3) + (v.vertex.z * _OffsetZ3));
            half value4 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX4) + (v.vertex.z * _OffsetZ4));
            half value5 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX5) + (v.vertex.z * _OffsetZ5));
            half value6 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX6) + (v.vertex.z * _OffsetZ6));
            half value7 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX7) + (v.vertex.z * _OffsetZ7));
            half value8 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert + (v.vertex.x * _OffsetX8) + (v.vertex.z * _OffsetZ8));

            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            
            // 対角サイン波２追加
            if(_DiagnalWave2){
            v.vertex.y += value0;
            v.normal.z -= value0;
            }

            if(sqrt(pow(worldPos.x - _xImpact1, 2) + pow(worldPos.z - _zImpact1, 2)) < _Distance1)
            {
            v.vertex.y += value1 * _WaveAmplitude1;
            v.normal.z -= value1 * _WaveAmplitude1;
            }

            if(sqrt(pow(worldPos.x - _xImpact2, 2) + pow(worldPos.z - _zImpact2, 2)) < _Distance2)
            {
            v.vertex.y += value2 * _WaveAmplitude2;
            v.normal.z -= value2 * _WaveAmplitude2;
            }

            if(sqrt(pow(worldPos.x - _xImpact3, 2) + pow(worldPos.z - _zImpact3, 2)) < _Distance3)
            {
            v.vertex.y += value3 * _WaveAmplitude3;
            v.normal.z -= value3 * _WaveAmplitude3;
            }

            if(sqrt(pow(worldPos.x - _xImpact4, 2) + pow(worldPos.z - _zImpact4, 2)) < _Distance4)
            {
            v.vertex.y += value4 * _WaveAmplitude4;
            v.normal.z -= value4 * _WaveAmplitude4;
            }

            if(sqrt(pow(worldPos.x - _xImpact5, 2) + pow(worldPos.z - _zImpact5, 2)) < _Distance5)
            {
            v.vertex.y += value5 * _WaveAmplitude5;
            v.normal.z -= value5 * _WaveAmplitude5;
            }

            if(sqrt(pow(worldPos.x - _xImpact6, 2) + pow(worldPos.z - _zImpact6, 2)) < _Distance6)
            {
            v.vertex.y += value6 * _WaveAmplitude6;
            v.normal.z -= value6 * _WaveAmplitude6;
            }

            if(sqrt(pow(worldPos.x - _xImpact7, 2) + pow(worldPos.z - _zImpact7, 2)) < _Distance7)
            {
            v.vertex.y += value7 * _WaveAmplitude7;
            v.normal.z -= value7 * _WaveAmplitude7;
            }

            if(sqrt(pow(worldPos.x - _xImpact8, 2) + pow(worldPos.z - _zImpact8, 2)) < _Distance8)
            {
            v.vertex.y += value8 * _WaveAmplitude8;
            v.normal.z -= value8 * _WaveAmplitude8;
            }

        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = _Color.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
