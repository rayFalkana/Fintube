namespace DesktopAppLowLevelKeyboardHook
{
	public enum RawKey : uint
	{
		_backspace = 0x08,
		_tab = 0x09,
		_enter = 0x0D,
		_pause = 0x13,
		_capsLock = 0x14,
		_escape = 0x1B,
		_space = 0x20,
		_pageUp = 0x21,
		_pageDown = 0x22,
		_end = 0x23,
		_home = 0x24,
		_leftArrow = 0x25,
		_upArrow = 0x26,
		_rightArrow = 0x27,
		_downArrow = 0x28,
		Snapshot = 0x2C,
		_insert = 0x2D,
		_delete = 0x2E,
		_0 = 0x30,
		_1 = 0x31,
		_2 = 0x32,
		_3 = 0x33,
		_4 = 0x34,
		_5 = 0x35,
		_6 = 0x36,
		_7 = 0x37,
		_8 = 0x38,
		_9 = 0x39,
		_a = 0x41,
		_b = 0x42,
		_c = 0x43,
		_d = 0x44,
		_e = 0x45,
		_f = 0x46,
		_g = 0x47,
		_h = 0x48,
		_i = 0x49,
		_j = 0x4A,
		_k = 0x4B,
		_l = 0x4C,
		_m = 0x4D,
		_n = 0x4E,
		_o = 0x4F,
		_p = 0x50,
		_q = 0x51,
		_r = 0x52,
		_s = 0x53,
		_t = 0x54,
		_u = 0x55,
		_v = 0x56,
		_w = 0x57,
		_x = 0x58,
		_y = 0x59,
		_z = 0x5A,
		LeftWindows = 0x5B,
		RightWindows = 0x5C,
		Application = 0x5D,
		_numpad0 = 0x60,
		_numpad1 = 0x61,
		_numpad2 = 0x62,
		_numpad3 = 0x63,
		_numpad4 = 0x64,
		_numpad5 = 0x65,
		_numpad6 = 0x66,
		_numpad7 = 0x67,
		_numpad8 = 0x68,
		_numpad9 = 0x69,
		_numpadMultiply = 0x6A,
		_numpadPlus = 0x6B,
		_numpadMinus = 0x6D,
		_numpadPeriod = 0x6E,
		_numpadDivide = 0x6F,
		_f1 = 0x70,
		_f2 = 0x71,
		_f3 = 0x72,
		_f4 = 0x73,
		_f5 = 0x74,
		_f6 = 0x75,
		_f7 = 0x76,
		_f8 = 0x77,
		_f9 = 0x78,
		_f10 = 0x79,
		_f11 = 0x7A,
		_f12 = 0x7B,
		_f13 = 0x7C,
		_f14 = 0x7D,
		_f15 = 0x7E,
		_f16 = 0x7F,
		_f17 = 0x80,
		_f18 = 0x81,
		_f19 = 0x82,
		_f20 = 0x83,
		_f21 = 0x84,
		_f22 = 0x85,
		_f23 = 0x86,
		_f24 = 0x87,
		_numLock = 0x90,
		_scrollLock = 0x91,
		_leftShift = 0xA0,
		_rightShift = 0xA1,
		_leftCtrl = 0xA2,
		_rightCtrl = 0xA3,
		LeftMenu = 0xA4,
		RightMenu = 0xA5,
		OEM1 = 0xBA,
		_equals = 0xBB,
		_comma = 0xBC,
		_minus = 0xBD,
		_period = 0xBE,
		OEM2 = 0xBF,
		OEM3 = 0xC0,
		OEM4 = 0xDB,
		OEM5 = 0xDC,
		OEM6 = 0xDD,
		OEM7 = 0xDE,

		// Advanced (scan code)
		International2 = 0x070 << 8,
		International4 = 0x079 << 8,
		International3 = 0x07D << 8

	}
	//https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
	//https://github.com/Elringus/UnityRawInput/blob/master/Assets/UnityRawInput/Runtime/RawKey.cs


	public static class TranslateKey
	{
		public static string Cut(string _key)
		{
			switch (_key)
			{
				case "numpad0":
					_key = "num0";
					break;
				case "numpad1":
					_key = "num1";
					break;
				case "numpad2":
					_key = "num2";
					break;
				case "numpad3":
					_key = "num3";
					break;
				case "numpad4":
					_key = "num4";
					break;
				case "numpad5":
					_key = "num5";
					break;
				case "numpad6":
					_key = "num6";
					break;
				case "numpad7":
					_key = "num7";
					break;
				case "numpad8":
					_key = "num8";
					break;
				case "numpad9":
					_key = "num9";
					break;
				case "numpadMultiply":
					_key = "num*";
					break;
				case "numpadPlus":
					_key = "num+";
					break;
				case "numpadMinus":
					_key = "num-";
					break;
				case "numpadPeriod":
					_key = "num.";
					break;
				case "numpadDivide":
					_key = "num/";
					break;
				case "numpadEnter":
					_key = "numEnter";
					break;
				case "comma":
					_key = ",";
					break;
				case "minus":
					_key = "-";
					break;
				case "equals":
					_key = "=";
					break;
				case "period":
					_key = ".";
					break;

			}
			return _key;
		}
	}
}