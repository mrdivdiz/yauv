fixed4 SampleYCbCr ( half2 Yuv, half2 CbCruv)
{
	#ifdef UNITY_COMPILER_CG
		fixed4 YCrCb = fixed4(tex2D (_YTex, Yuv).r + 0.001, tex2D (_CrTex, CbCruv).g + 0.001, tex2D (_CbTex, CbCruv).b + 0.001, 1.0);
	#else
		fixed4 YCrCb = fixed4(tex2D (_YTex, Yuv).r, tex2D (_CrTex, CbCruv).g, tex2D (_CbTex, CbCruv).b, 1.0);
	#endif
	
	return YCrCb;
}


fixed4 YCbCrToRGB( fixed4 YCbCr )
{
	//Spent ages on these
	//www.theora.org/doc/Theora.pdf
	//R = ((Y - (16.0/255.0)) * (255.0/219.0)) + (2*(1 - 0.299)*((Cr - (128.0/255.0)) * (255.0/244.0)))
	//G = ((Y - (16.0/255.0)) * (255.0/219.0)) - (2 * (((1 - 0.114)*0.114)/(1 - 0.114 - 0.299)) * ((Cb - (128.0/255.0)) * (255.0/244.0))) - (2 * (((1 - 0.299)*0.299)/(1 - 0.114 - 0.299)) * ((Cr - (128.0/255.0)) * (255.0/244.0))) 
	//B = ((Y - (16.0/255.0)) * (255.0/219.0)) + (2*(1 - 0.114)*((Cb - (128.0/255.0)) * (255.0/244.0)))

	//fixed4 YCbCr2R = fixed4(1.16438, 1.4652, 0, -0.808535);
	//fixed4 YCbCr2G = fixed4(1.16438, -0.714136, -0.359651, 0.46594);
	//fixed4 YCbCr2B = fixed4(1.16438, 0,1.85189, -1.00263);
	
	//However the original ones are more accurate with a colour bar test video
	fixed4 YCbCr2R = fixed4(1.1643828125, 1.59602734375, 0, -.87078515625);
	fixed4 YCbCr2G = fixed4(1.1643828125, -.81296875, -.39176171875, .52959375);
	fixed4 YCbCr2B = fixed4(1.1643828125, 0, 2.017234375,  -1.081390625);
	
	fixed4 rgbVec;

	rgbVec.x = dot(YCbCr2R, YCbCr);
	rgbVec.y = dot(YCbCr2G, YCbCr);
	rgbVec.z = dot(YCbCr2B, YCbCr);
	rgbVec.w = 1.0f;
	
	return rgbVec;
}