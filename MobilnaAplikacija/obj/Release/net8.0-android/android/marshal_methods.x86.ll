; ModuleID = 'marshal_methods.x86.ll'
source_filename = "marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [125 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [250 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 99
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 98
	i32 42639949, ; 2: System.Threading.Thread => 0x28aa24d => 117
	i32 67008169, ; 3: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 33
	i32 72070932, ; 4: Microsoft.Maui.Graphics.dll => 0x44bb714 => 51
	i32 98325684, ; 5: Microsoft.Extensions.Diagnostics.Abstractions => 0x5dc54b4 => 41
	i32 117431740, ; 6: System.Runtime.InteropServices => 0x6ffddbc => 108
	i32 122350210, ; 7: System.Threading.Channels.dll => 0x74aea82 => 116
	i32 165246403, ; 8: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 57
	i32 182336117, ; 9: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 76
	i32 195452805, ; 10: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 30
	i32 199333315, ; 11: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 31
	i32 205061960, ; 12: System.ComponentModel => 0xc38ff48 => 88
	i32 221958352, ; 13: Microsoft.Extensions.Diagnostics.dll => 0xd3ad0d0 => 40
	i32 280992041, ; 14: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 2
	i32 291275502, ; 15: Microsoft.Extensions.Http.dll => 0x115c82ee => 42
	i32 317674968, ; 16: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 30
	i32 318968648, ; 17: Xamarin.AndroidX.Activity.dll => 0x13031348 => 53
	i32 336156722, ; 18: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 15
	i32 342366114, ; 19: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 64
	i32 356389973, ; 20: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 14
	i32 379916513, ; 21: System.Threading.Thread.dll => 0x16a510e1 => 117
	i32 385762202, ; 22: System.Memory.dll => 0x16fe439a => 95
	i32 395744057, ; 23: _Microsoft.Android.Resource.Designer => 0x17969339 => 34
	i32 435591531, ; 24: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 26
	i32 442565967, ; 25: System.Collections => 0x1a61054f => 85
	i32 450948140, ; 26: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 63
	i32 469710990, ; 27: System.dll => 0x1bff388e => 120
	i32 498788369, ; 28: System.ObjectModel => 0x1dbae811 => 105
	i32 500358224, ; 29: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 13
	i32 503918385, ; 30: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 7
	i32 513247710, ; 31: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 46
	i32 539058512, ; 32: Microsoft.Extensions.Logging => 0x20216150 => 43
	i32 592146354, ; 33: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 21
	i32 627609679, ; 34: Xamarin.AndroidX.CustomView => 0x2568904f => 61
	i32 627931235, ; 35: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 19
	i32 662205335, ; 36: System.Text.Encodings.Web.dll => 0x27787397 => 113
	i32 672442732, ; 37: System.Collections.Concurrent => 0x2814a96c => 83
	i32 683518922, ; 38: System.Net.Security => 0x28bdabca => 102
	i32 688181140, ; 39: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 1
	i32 706645707, ; 40: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 16
	i32 709557578, ; 41: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 4
	i32 722857257, ; 42: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 759454413, ; 43: System.Net.Requests => 0x2d445acd => 101
	i32 775507847, ; 44: System.IO.Compression => 0x2e394f87 => 92
	i32 777317022, ; 45: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 25
	i32 789151979, ; 46: Microsoft.Extensions.Options => 0x2f0980eb => 45
	i32 823281589, ; 47: System.Private.Uri.dll => 0x311247b5 => 106
	i32 830298997, ; 48: System.IO.Compression.Brotli => 0x317d5b75 => 91
	i32 878954865, ; 49: System.Net.Http.Json => 0x3463c971 => 96
	i32 904024072, ; 50: System.ComponentModel.Primitives.dll => 0x35e25008 => 86
	i32 926902833, ; 51: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 28
	i32 967690846, ; 52: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 64
	i32 992768348, ; 53: System.Collections.dll => 0x3b2c715c => 85
	i32 1012816738, ; 54: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 74
	i32 1028951442, ; 55: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 39
	i32 1029334545, ; 56: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 3
	i32 1035644815, ; 57: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 54
	i32 1044663988, ; 58: System.Linq.Expressions.dll => 0x3e444eb4 => 93
	i32 1048992957, ; 59: Microsoft.Extensions.Diagnostics.Abstractions.dll => 0x3e865cbd => 41
	i32 1052210849, ; 60: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 66
	i32 1082857460, ; 61: System.ComponentModel.TypeConverter => 0x408b17f4 => 87
	i32 1084122840, ; 62: Xamarin.Kotlin.StdLib => 0x409e66d8 => 80
	i32 1098259244, ; 63: System => 0x41761b2c => 120
	i32 1118262833, ; 64: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 16
	i32 1168523401, ; 65: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 22
	i32 1178241025, ; 66: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 71
	i32 1203215381, ; 67: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 20
	i32 1214827643, ; 68: CommunityToolkit.Mvvm => 0x4868cc7b => 35
	i32 1234928153, ; 69: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 18
	i32 1260983243, ; 70: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 2
	i32 1293217323, ; 71: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 62
	i32 1324164729, ; 72: System.Linq => 0x4eed2679 => 94
	i32 1373134921, ; 73: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 32
	i32 1376866003, ; 74: Xamarin.AndroidX.SavedState => 0x52114ed3 => 74
	i32 1406073936, ; 75: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 58
	i32 1430672901, ; 76: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 0
	i32 1452070440, ; 77: System.Formats.Asn1.dll => 0x568cd628 => 90
	i32 1458022317, ; 78: System.Net.Security.dll => 0x56e7a7ad => 102
	i32 1461004990, ; 79: es\Microsoft.Maui.Controls.resources => 0x57152abe => 6
	i32 1462112819, ; 80: System.IO.Compression.dll => 0x57261233 => 92
	i32 1469204771, ; 81: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 55
	i32 1470490898, ; 82: Microsoft.Extensions.Primitives => 0x57a5e912 => 46
	i32 1480492111, ; 83: System.IO.Compression.Brotli.dll => 0x583e844f => 91
	i32 1493001747, ; 84: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 10
	i32 1505131794, ; 85: Microsoft.Extensions.Http => 0x59b67d12 => 42
	i32 1514721132, ; 86: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 5
	i32 1543031311, ; 87: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 115
	i32 1551623176, ; 88: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 25
	i32 1622152042, ; 89: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 68
	i32 1624863272, ; 90: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 78
	i32 1636350590, ; 91: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 60
	i32 1639515021, ; 92: System.Net.Http.dll => 0x61b9038d => 97
	i32 1639986890, ; 93: System.Text.RegularExpressions => 0x61c036ca => 115
	i32 1657153582, ; 94: System.Runtime => 0x62c6282e => 111
	i32 1658251792, ; 95: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 79
	i32 1677501392, ; 96: System.Net.Primitives.dll => 0x63fca3d0 => 100
	i32 1679769178, ; 97: System.Security.Cryptography => 0x641f3e5a => 112
	i32 1729485958, ; 98: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 56
	i32 1736233607, ; 99: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 23
	i32 1743415430, ; 100: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 1
	i32 1766324549, ; 101: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 76
	i32 1770582343, ; 102: Microsoft.Extensions.Logging.dll => 0x6988f147 => 43
	i32 1780572499, ; 103: Mono.Android.Runtime.dll => 0x6a216153 => 123
	i32 1782862114, ; 104: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 17
	i32 1788241197, ; 105: Xamarin.AndroidX.Fragment => 0x6a96652d => 63
	i32 1793755602, ; 106: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 9
	i32 1808609942, ; 107: Xamarin.AndroidX.Loader => 0x6bcd3296 => 68
	i32 1813058853, ; 108: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 80
	i32 1813201214, ; 109: Xamarin.Google.Android.Material => 0x6c13413e => 79
	i32 1818569960, ; 110: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 72
	i32 1828688058, ; 111: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 44
	i32 1842015223, ; 112: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 29
	i32 1853025655, ; 113: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 26
	i32 1858542181, ; 114: System.Linq.Expressions => 0x6ec71a65 => 93
	i32 1875935024, ; 115: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 8
	i32 1910275211, ; 116: System.Collections.NonGeneric.dll => 0x71dc7c8b => 84
	i32 1924095012, ; 117: MobilnaAplikacija => 0x72af5c24 => 82
	i32 1961813231, ; 118: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 75
	i32 1968388702, ; 119: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 36
	i32 2003115576, ; 120: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 5
	i32 2019465201, ; 121: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 66
	i32 2025202353, ; 122: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 0
	i32 2045470958, ; 123: System.Private.Xml => 0x79eb68ee => 107
	i32 2055257422, ; 124: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 65
	i32 2066184531, ; 125: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 4
	i32 2079903147, ; 126: System.Runtime.dll => 0x7bf8cdab => 111
	i32 2090596640, ; 127: System.Numerics.Vectors => 0x7c9bf920 => 104
	i32 2127167465, ; 128: System.Console => 0x7ec9ffe9 => 89
	i32 2159891885, ; 129: Microsoft.Maui => 0x80bd55ad => 49
	i32 2169148018, ; 130: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 12
	i32 2181898931, ; 131: Microsoft.Extensions.Options.dll => 0x820d22b3 => 45
	i32 2192057212, ; 132: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 44
	i32 2193016926, ; 133: System.ObjectModel.dll => 0x82b6c85e => 105
	i32 2201107256, ; 134: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 81
	i32 2201231467, ; 135: System.Net.Http => 0x8334206b => 97
	i32 2207618523, ; 136: it\Microsoft.Maui.Controls.resources => 0x839595db => 14
	i32 2266799131, ; 137: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 37
	i32 2270573516, ; 138: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 8
	i32 2279755925, ; 139: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 73
	i32 2295906218, ; 140: System.Net.Sockets => 0x88d8bfaa => 103
	i32 2303942373, ; 141: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 18
	i32 2305521784, ; 142: System.Private.CoreLib.dll => 0x896b7878 => 121
	i32 2353062107, ; 143: System.Net.Primitives => 0x8c40e0db => 100
	i32 2368005991, ; 144: System.Xml.ReaderWriter.dll => 0x8d24e767 => 119
	i32 2371007202, ; 145: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 36
	i32 2395872292, ; 146: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 13
	i32 2427813419, ; 147: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 10
	i32 2435356389, ; 148: System.Console.dll => 0x912896e5 => 89
	i32 2458678730, ; 149: System.Net.Sockets.dll => 0x928c75ca => 103
	i32 2475788418, ; 150: Java.Interop.dll => 0x93918882 => 122
	i32 2480646305, ; 151: Microsoft.Maui.Controls => 0x93dba8a1 => 47
	i32 2550873716, ; 152: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 11
	i32 2570120770, ; 153: System.Text.Encodings.Web => 0x9930ee42 => 113
	i32 2593496499, ; 154: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 20
	i32 2605712449, ; 155: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 81
	i32 2617129537, ; 156: System.Private.Xml.dll => 0x9bfe3a41 => 107
	i32 2620871830, ; 157: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 60
	i32 2626831493, ; 158: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 15
	i32 2663698177, ; 159: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2724373263, ; 160: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 161: Xamarin.AndroidX.Activity => 0xa2e0939b => 53
	i32 2735172069, ; 162: System.Threading.Channels => 0xa30769e5 => 116
	i32 2737747696, ; 163: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 55
	i32 2752995522, ; 164: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 21
	i32 2758225723, ; 165: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 48
	i32 2764765095, ; 166: Microsoft.Maui.dll => 0xa4caf7a7 => 49
	i32 2778768386, ; 167: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 77
	i32 2785988530, ; 168: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 27
	i32 2801831435, ; 169: Microsoft.Maui.Graphics => 0xa7008e0b => 51
	i32 2806116107, ; 170: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 6
	i32 2810250172, ; 171: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 58
	i32 2831556043, ; 172: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 19
	i32 2853208004, ; 173: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 77
	i32 2861189240, ; 174: Microsoft.Maui.Essentials => 0xaa8a4878 => 50
	i32 2909740682, ; 175: System.Private.CoreLib => 0xad6f1e8a => 121
	i32 2916838712, ; 176: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 78
	i32 2919462931, ; 177: System.Numerics.Vectors.dll => 0xae037813 => 104
	i32 2959614098, ; 178: System.ComponentModel.dll => 0xb0682092 => 88
	i32 2978675010, ; 179: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 62
	i32 2987532451, ; 180: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 75
	i32 3020703001, ; 181: Microsoft.Extensions.Diagnostics => 0xb40c4519 => 40
	i32 3038032645, ; 182: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 34
	i32 3057625584, ; 183: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 69
	i32 3059408633, ; 184: Mono.Android.Runtime => 0xb65adef9 => 123
	i32 3059793426, ; 185: System.ComponentModel.Primitives => 0xb660be12 => 86
	i32 3077302341, ; 186: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 12
	i32 3103600923, ; 187: System.Formats.Asn1 => 0xb8fd311b => 90
	i32 3178803400, ; 188: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 70
	i32 3220365878, ; 189: System.Threading => 0xbff2e236 => 118
	i32 3258312781, ; 190: Xamarin.AndroidX.CardView => 0xc235e84d => 56
	i32 3305363605, ; 191: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 7
	i32 3316684772, ; 192: System.Net.Requests.dll => 0xc5b097e4 => 101
	i32 3317135071, ; 193: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 61
	i32 3346324047, ; 194: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 71
	i32 3357674450, ; 195: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 24
	i32 3358260929, ; 196: System.Text.Json => 0xc82afec1 => 114
	i32 3362522851, ; 197: Xamarin.AndroidX.Core => 0xc86c06e3 => 59
	i32 3366347497, ; 198: Java.Interop => 0xc8a662e9 => 122
	i32 3374999561, ; 199: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 73
	i32 3381016424, ; 200: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 3
	i32 3428513518, ; 201: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 38
	i32 3463511458, ; 202: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 11
	i32 3471940407, ; 203: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 87
	i32 3476120550, ; 204: Mono.Android => 0xcf3163e6 => 124
	i32 3479583265, ; 205: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 24
	i32 3484440000, ; 206: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 23
	i32 3485117614, ; 207: System.Text.Json.dll => 0xcfbaacae => 114
	i32 3580758918, ; 208: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 31
	i32 3608519521, ; 209: System.Linq.dll => 0xd715a361 => 94
	i32 3641597786, ; 210: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 65
	i32 3643446276, ; 211: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 28
	i32 3643854240, ; 212: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 70
	i32 3657292374, ; 213: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 37
	i32 3660523487, ; 214: System.Net.NetworkInformation => 0xda2f27df => 99
	i32 3672681054, ; 215: Mono.Android.dll => 0xdae8aa5e => 124
	i32 3697841164, ; 216: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 33
	i32 3724971120, ; 217: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 69
	i32 3732100267, ; 218: System.Net.NameResolution => 0xde7354ab => 98
	i32 3737834244, ; 219: System.Net.Http.Json.dll => 0xdecad304 => 96
	i32 3748608112, ; 220: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 52
	i32 3786282454, ; 221: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 57
	i32 3792276235, ; 222: System.Collections.NonGeneric => 0xe2098b0b => 84
	i32 3823082795, ; 223: System.Security.Cryptography.dll => 0xe3df9d2b => 112
	i32 3841636137, ; 224: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 39
	i32 3849253459, ; 225: System.Runtime.InteropServices.dll => 0xe56ef253 => 108
	i32 3889960447, ; 226: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 32
	i32 3896106733, ; 227: System.Collections.Concurrent.dll => 0xe839deed => 83
	i32 3896760992, ; 228: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 59
	i32 3928044579, ; 229: System.Xml.ReaderWriter => 0xea213423 => 119
	i32 3931092270, ; 230: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 72
	i32 3955647286, ; 231: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 54
	i32 3980434154, ; 232: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 27
	i32 3987592930, ; 233: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 9
	i32 4025784931, ; 234: System.Memory => 0xeff49a63 => 95
	i32 4046471985, ; 235: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 48
	i32 4073602200, ; 236: System.Threading.dll => 0xf2ce3c98 => 118
	i32 4094352644, ; 237: Microsoft.Maui.Essentials.dll => 0xf40add04 => 50
	i32 4100113165, ; 238: System.Private.Uri => 0xf462c30d => 106
	i32 4102112229, ; 239: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 22
	i32 4125707920, ; 240: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 17
	i32 4126470640, ; 241: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 38
	i32 4131205717, ; 242: MobilnaAplikacija.dll => 0xf63d3255 => 82
	i32 4150914736, ; 243: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 29
	i32 4182413190, ; 244: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 67
	i32 4213026141, ; 245: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 52
	i32 4271975918, ; 246: Microsoft.Maui.Controls.dll => 0xfea12dee => 47
	i32 4274623895, ; 247: CommunityToolkit.Mvvm.dll => 0xfec99597 => 35
	i32 4274976490, ; 248: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959 ; 249: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 67
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [250 x i32] [
	i32 99, ; 0
	i32 98, ; 1
	i32 117, ; 2
	i32 33, ; 3
	i32 51, ; 4
	i32 41, ; 5
	i32 108, ; 6
	i32 116, ; 7
	i32 57, ; 8
	i32 76, ; 9
	i32 30, ; 10
	i32 31, ; 11
	i32 88, ; 12
	i32 40, ; 13
	i32 2, ; 14
	i32 42, ; 15
	i32 30, ; 16
	i32 53, ; 17
	i32 15, ; 18
	i32 64, ; 19
	i32 14, ; 20
	i32 117, ; 21
	i32 95, ; 22
	i32 34, ; 23
	i32 26, ; 24
	i32 85, ; 25
	i32 63, ; 26
	i32 120, ; 27
	i32 105, ; 28
	i32 13, ; 29
	i32 7, ; 30
	i32 46, ; 31
	i32 43, ; 32
	i32 21, ; 33
	i32 61, ; 34
	i32 19, ; 35
	i32 113, ; 36
	i32 83, ; 37
	i32 102, ; 38
	i32 1, ; 39
	i32 16, ; 40
	i32 4, ; 41
	i32 109, ; 42
	i32 101, ; 43
	i32 92, ; 44
	i32 25, ; 45
	i32 45, ; 46
	i32 106, ; 47
	i32 91, ; 48
	i32 96, ; 49
	i32 86, ; 50
	i32 28, ; 51
	i32 64, ; 52
	i32 85, ; 53
	i32 74, ; 54
	i32 39, ; 55
	i32 3, ; 56
	i32 54, ; 57
	i32 93, ; 58
	i32 41, ; 59
	i32 66, ; 60
	i32 87, ; 61
	i32 80, ; 62
	i32 120, ; 63
	i32 16, ; 64
	i32 22, ; 65
	i32 71, ; 66
	i32 20, ; 67
	i32 35, ; 68
	i32 18, ; 69
	i32 2, ; 70
	i32 62, ; 71
	i32 94, ; 72
	i32 32, ; 73
	i32 74, ; 74
	i32 58, ; 75
	i32 0, ; 76
	i32 90, ; 77
	i32 102, ; 78
	i32 6, ; 79
	i32 92, ; 80
	i32 55, ; 81
	i32 46, ; 82
	i32 91, ; 83
	i32 10, ; 84
	i32 42, ; 85
	i32 5, ; 86
	i32 115, ; 87
	i32 25, ; 88
	i32 68, ; 89
	i32 78, ; 90
	i32 60, ; 91
	i32 97, ; 92
	i32 115, ; 93
	i32 111, ; 94
	i32 79, ; 95
	i32 100, ; 96
	i32 112, ; 97
	i32 56, ; 98
	i32 23, ; 99
	i32 1, ; 100
	i32 76, ; 101
	i32 43, ; 102
	i32 123, ; 103
	i32 17, ; 104
	i32 63, ; 105
	i32 9, ; 106
	i32 68, ; 107
	i32 80, ; 108
	i32 79, ; 109
	i32 72, ; 110
	i32 44, ; 111
	i32 29, ; 112
	i32 26, ; 113
	i32 93, ; 114
	i32 8, ; 115
	i32 84, ; 116
	i32 82, ; 117
	i32 75, ; 118
	i32 36, ; 119
	i32 5, ; 120
	i32 66, ; 121
	i32 0, ; 122
	i32 107, ; 123
	i32 65, ; 124
	i32 4, ; 125
	i32 111, ; 126
	i32 104, ; 127
	i32 89, ; 128
	i32 49, ; 129
	i32 12, ; 130
	i32 45, ; 131
	i32 44, ; 132
	i32 105, ; 133
	i32 81, ; 134
	i32 97, ; 135
	i32 14, ; 136
	i32 37, ; 137
	i32 8, ; 138
	i32 73, ; 139
	i32 103, ; 140
	i32 18, ; 141
	i32 121, ; 142
	i32 100, ; 143
	i32 119, ; 144
	i32 36, ; 145
	i32 13, ; 146
	i32 10, ; 147
	i32 89, ; 148
	i32 103, ; 149
	i32 122, ; 150
	i32 47, ; 151
	i32 11, ; 152
	i32 113, ; 153
	i32 20, ; 154
	i32 81, ; 155
	i32 107, ; 156
	i32 60, ; 157
	i32 15, ; 158
	i32 109, ; 159
	i32 110, ; 160
	i32 53, ; 161
	i32 116, ; 162
	i32 55, ; 163
	i32 21, ; 164
	i32 48, ; 165
	i32 49, ; 166
	i32 77, ; 167
	i32 27, ; 168
	i32 51, ; 169
	i32 6, ; 170
	i32 58, ; 171
	i32 19, ; 172
	i32 77, ; 173
	i32 50, ; 174
	i32 121, ; 175
	i32 78, ; 176
	i32 104, ; 177
	i32 88, ; 178
	i32 62, ; 179
	i32 75, ; 180
	i32 40, ; 181
	i32 34, ; 182
	i32 69, ; 183
	i32 123, ; 184
	i32 86, ; 185
	i32 12, ; 186
	i32 90, ; 187
	i32 70, ; 188
	i32 118, ; 189
	i32 56, ; 190
	i32 7, ; 191
	i32 101, ; 192
	i32 61, ; 193
	i32 71, ; 194
	i32 24, ; 195
	i32 114, ; 196
	i32 59, ; 197
	i32 122, ; 198
	i32 73, ; 199
	i32 3, ; 200
	i32 38, ; 201
	i32 11, ; 202
	i32 87, ; 203
	i32 124, ; 204
	i32 24, ; 205
	i32 23, ; 206
	i32 114, ; 207
	i32 31, ; 208
	i32 94, ; 209
	i32 65, ; 210
	i32 28, ; 211
	i32 70, ; 212
	i32 37, ; 213
	i32 99, ; 214
	i32 124, ; 215
	i32 33, ; 216
	i32 69, ; 217
	i32 98, ; 218
	i32 96, ; 219
	i32 52, ; 220
	i32 57, ; 221
	i32 84, ; 222
	i32 112, ; 223
	i32 39, ; 224
	i32 108, ; 225
	i32 32, ; 226
	i32 83, ; 227
	i32 59, ; 228
	i32 119, ; 229
	i32 72, ; 230
	i32 54, ; 231
	i32 27, ; 232
	i32 9, ; 233
	i32 95, ; 234
	i32 48, ; 235
	i32 118, ; 236
	i32 50, ; 237
	i32 106, ; 238
	i32 22, ; 239
	i32 17, ; 240
	i32 38, ; 241
	i32 82, ; 242
	i32 29, ; 243
	i32 67, ; 244
	i32 52, ; 245
	i32 47, ; 246
	i32 35, ; 247
	i32 110, ; 248
	i32 67 ; 249
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ a8cd27e430e55df3e3c1e3a43d35c11d9512a2db"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"NumRegisterParameters", i32 0}
