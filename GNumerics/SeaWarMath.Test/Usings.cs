global using System;
global using Xunit;
global using System.Diagnostics;
global using System.Collections.Generic;
global using System.Runtime.CompilerServices;

#if USE_FIXED64
global using Double = Gal.Core.Fixed40_24;
global using Single = Gal.Core.Fixed64;
#else
global using Double = System.Double;
global using Single = System.Single;
#endif
