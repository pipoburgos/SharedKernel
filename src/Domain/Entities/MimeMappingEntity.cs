﻿// ReSharper disable StringLiteralTypo

namespace SharedKernel.Domain.Entities;

/// <summary>
/// 
/// </summary>
public static class MimeMappingEntity
{
    private static readonly MimeMappingDictionaryBase MappingDictionary = new MimeMappingDictionaryClassic();

    /// <summary>Returns the MIME mapping for the specified file name.</summary>
    /// <param name="fileName">The file name that is used to determine the MIME type.</param>
    public static string GetMimeMapping(string fileName)
    {
        if (fileName == null)
            throw new ArgumentNullException(nameof(fileName));

        return MappingDictionary.GetMimeMappingPrivate(fileName);
    }

    private abstract class MimeMappingDictionaryBase
    {
        private static readonly char[] PathSeparatorChars =
        [
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar,
            Path.VolumeSeparatorChar,
        ];

        private readonly Dictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private bool _isInitialized;

        protected void AddMapping(string fileExtension, string mimeType)
        {
            _mappings.Add(fileExtension, mimeType);
        }

        private void AddWildcardIfNotPresent()
        {
            if (_mappings.ContainsKey(".*"))
                return;

            AddMapping(".*", "application/octet-stream");
        }

        private void EnsureMapping()
        {
            if (_isInitialized)
                return;

            lock (this)
            {
                if (_isInitialized)
                    return;

                PopulateMappings();
                AddWildcardIfNotPresent();
                _isInitialized = true;
            }
        }

        protected abstract void PopulateMappings();

        private static string GetFileName(string path)
        {
            var startIndex = path.LastIndexOfAny(PathSeparatorChars);
            return startIndex < 0 ? path : path.Substring(startIndex);
        }

        public string GetMimeMappingPrivate(string fileName)
        {
            EnsureMapping();
            fileName = GetFileName(fileName);
            for (var startIndex = 0; startIndex < fileName.Length; ++startIndex)
            {
                if (fileName[startIndex] == '.' && _mappings.TryGetValue(fileName.Substring(startIndex), out var str))
                    return str;
            }
            return _mappings[".*"];
        }
    }

    private sealed class MimeMappingDictionaryClassic : MimeMappingDictionaryBase
    {
        protected override void PopulateMappings()
        {
            AddMapping(".323", "text/h323");
            AddMapping(".aaf", "application/octet-stream");
            AddMapping(".aca", "application/octet-stream");
            AddMapping(".accdb", "application/msaccess");
            AddMapping(".accde", "application/msaccess");
            AddMapping(".accdt", "application/msaccess");
            AddMapping(".acx", "application/internet-property-stream");
            AddMapping(".afm", "application/octet-stream");
            AddMapping(".ai", "application/postscript");
            AddMapping(".aif", "audio/x-aiff");
            AddMapping(".aifc", "audio/aiff");
            AddMapping(".aiff", "audio/aiff");
            AddMapping(".application", "application/x-ms-application");
            AddMapping(".art", "image/x-jg");
            AddMapping(".asd", "application/octet-stream");
            AddMapping(".asf", "video/x-ms-asf");
            AddMapping(".asi", "application/octet-stream");
            AddMapping(".asm", "text/plain");
            AddMapping(".asr", "video/x-ms-asf");
            AddMapping(".asx", "video/x-ms-asf");
            AddMapping(".atom", "application/atom+xml");
            AddMapping(".au", "audio/basic");
            AddMapping(".avi", "video/x-msvideo");
            AddMapping(".axs", "application/olescript");
            AddMapping(".bas", "text/plain");
            AddMapping(".bcpio", "application/x-bcpio");
            AddMapping(".bin", "application/octet-stream");
            AddMapping(".bmp", "image/bmp");
            AddMapping(".c", "text/plain");
            AddMapping(".cab", "application/octet-stream");
            AddMapping(".calx", "application/vnd.ms-office.calx");
            AddMapping(".cat", "application/vnd.ms-pki.seccat");
            AddMapping(".cdf", "application/x-cdf");
            AddMapping(".chm", "application/octet-stream");
            AddMapping(".class", "application/x-java-applet");
            AddMapping(".clp", "application/x-msclip");
            AddMapping(".cmx", "image/x-cmx");
            AddMapping(".cnf", "text/plain");
            AddMapping(".cod", "image/cis-cod");
            AddMapping(".cpio", "application/x-cpio");
            AddMapping(".cpp", "text/plain");
            AddMapping(".crd", "application/x-mscardfile");
            AddMapping(".crl", "application/pkix-crl");
            AddMapping(".crt", "application/x-x509-ca-cert");
            AddMapping(".csh", "application/x-csh");
            AddMapping(".css", "text/css");
            AddMapping(".csv", "application/octet-stream");
            AddMapping(".cur", "application/octet-stream");
            AddMapping(".dcr", "application/x-director");
            AddMapping(".deploy", "application/octet-stream");
            AddMapping(".der", "application/x-x509-ca-cert");
            AddMapping(".dib", "image/bmp");
            AddMapping(".dir", "application/x-director");
            AddMapping(".disco", "text/xml");
            AddMapping(".dll", "application/x-msdownload");
            AddMapping(".dll.config", "text/xml");
            AddMapping(".dlm", "text/dlm");
            AddMapping(".doc", "application/msword");
            AddMapping(".docm", "application/vnd.ms-word.document.macroEnabled.12");
            AddMapping(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            AddMapping(".dot", "application/msword");
            AddMapping(".dotm", "application/vnd.ms-word.template.macroEnabled.12");
            AddMapping(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            AddMapping(".dsp", "application/octet-stream");
            AddMapping(".dtd", "text/xml");
            AddMapping(".dvi", "application/x-dvi");
            AddMapping(".dwf", "drawing/x-dwf");
            AddMapping(".dwp", "application/octet-stream");
            AddMapping(".dxr", "application/x-director");
            AddMapping(".eml", "message/rfc822");
            AddMapping(".emz", "application/octet-stream");
            AddMapping(".eot", "application/octet-stream");
            AddMapping(".eps", "application/postscript");
            AddMapping(".etx", "text/x-setext");
            AddMapping(".evy", "application/envoy");
            AddMapping(".exe", "application/octet-stream");
            AddMapping(".exe.config", "text/xml");
            AddMapping(".fdf", "application/vnd.fdf");
            AddMapping(".fif", "application/fractals");
            AddMapping(".fla", "application/octet-stream");
            AddMapping(".flr", "x-world/x-vrml");
            AddMapping(".flv", "video/x-flv");
            AddMapping(".gif", "image/gif");
            AddMapping(".gtar", "application/x-gtar");
            AddMapping(".gz", "application/x-gzip");
            AddMapping(".h", "text/plain");
            AddMapping(".hdf", "application/x-hdf");
            AddMapping(".hdml", "text/x-hdml");
            AddMapping(".hhc", "application/x-oleobject");
            AddMapping(".hhk", "application/octet-stream");
            AddMapping(".hhp", "application/octet-stream");
            AddMapping(".hlp", "application/winhlp");
            AddMapping(".hqx", "application/mac-binhex40");
            AddMapping(".hta", "application/hta");
            AddMapping(".htc", "text/x-component");
            AddMapping(".htm", "text/html");
            AddMapping(".html", "text/html");
            AddMapping(".htt", "text/webviewhtml");
            AddMapping(".hxt", "text/html");
            AddMapping(".ico", "image/x-icon");
            AddMapping(".ics", "application/octet-stream");
            AddMapping(".ief", "image/ief");
            AddMapping(".iii", "application/x-iphone");
            AddMapping(".inf", "application/octet-stream");
            AddMapping(".ins", "application/x-internet-signup");
            AddMapping(".isp", "application/x-internet-signup");
            AddMapping(".IVF", "video/x-ivf");
            AddMapping(".jar", "application/java-archive");
            AddMapping(".java", "application/octet-stream");
            AddMapping(".jck", "application/liquidmotion");
            AddMapping(".jcz", "application/liquidmotion");
            AddMapping(".jfif", "image/pjpeg");
            AddMapping(".jpb", "application/octet-stream");
            AddMapping(".jpe", "image/jpeg");
            AddMapping(".jpeg", "image/jpeg");
            AddMapping(".jpg", "image/jpeg");
            AddMapping(".js", "application/x-javascript");
            AddMapping(".jsx", "text/jscript");
            AddMapping(".latex", "application/x-latex");
            AddMapping(".lit", "application/x-ms-reader");
            AddMapping(".lpk", "application/octet-stream");
            AddMapping(".lsf", "video/x-la-asf");
            AddMapping(".lsx", "video/x-la-asf");
            AddMapping(".lzh", "application/octet-stream");
            AddMapping(".m13", "application/x-msmediaview");
            AddMapping(".m14", "application/x-msmediaview");
            AddMapping(".m1v", "video/mpeg");
            AddMapping(".m3u", "audio/x-mpegurl");
            AddMapping(".man", "application/x-troff-man");
            AddMapping(".manifest", "application/x-ms-manifest");
            AddMapping(".map", "text/plain");
            AddMapping(".mdb", "application/x-msaccess");
            AddMapping(".mdp", "application/octet-stream");
            AddMapping(".me", "application/x-troff-me");
            AddMapping(".mht", "message/rfc822");
            AddMapping(".mhtml", "message/rfc822");
            AddMapping(".mid", "audio/mid");
            AddMapping(".midi", "audio/mid");
            AddMapping(".mix", "application/octet-stream");
            AddMapping(".mmf", "application/x-smaf");
            AddMapping(".mno", "text/xml");
            AddMapping(".mny", "application/x-msmoney");
            AddMapping(".mov", "video/quicktime");
            AddMapping(".movie", "video/x-sgi-movie");
            AddMapping(".mp2", "video/mpeg");
            AddMapping(".mp3", "audio/mpeg");
            AddMapping(".mpa", "video/mpeg");
            AddMapping(".mpe", "video/mpeg");
            AddMapping(".mpeg", "video/mpeg");
            AddMapping(".mpg", "video/mpeg");
            AddMapping(".mpp", "application/vnd.ms-project");
            AddMapping(".mpv2", "video/mpeg");
            AddMapping(".ms", "application/x-troff-ms");
            AddMapping(".msi", "application/octet-stream");
            AddMapping(".mso", "application/octet-stream");
            AddMapping(".mvb", "application/x-msmediaview");
            AddMapping(".mvc", "application/x-miva-compiled");
            AddMapping(".nc", "application/x-netcdf");
            AddMapping(".nsc", "video/x-ms-asf");
            AddMapping(".nws", "message/rfc822");
            AddMapping(".ocx", "application/octet-stream");
            AddMapping(".oda", "application/oda");
            AddMapping(".odc", "text/x-ms-odc");
            AddMapping(".ods", "application/oleobject");
            AddMapping(".one", "application/onenote");
            AddMapping(".onea", "application/onenote");
            AddMapping(".onetoc", "application/onenote");
            AddMapping(".onetoc2", "application/onenote");
            AddMapping(".onetmp", "application/onenote");
            AddMapping(".onepkg", "application/onenote");
            AddMapping(".osdx", "application/opensearchdescription+xml");
            AddMapping(".p10", "application/pkcs10");
            AddMapping(".p12", "application/x-pkcs12");
            AddMapping(".p7b", "application/x-pkcs7-certificates");
            AddMapping(".p7c", "application/pkcs7-mime");
            AddMapping(".p7m", "application/pkcs7-mime");
            AddMapping(".p7r", "application/x-pkcs7-certreqresp");
            AddMapping(".p7s", "application/pkcs7-signature");
            AddMapping(".pbm", "image/x-portable-bitmap");
            AddMapping(".pcx", "application/octet-stream");
            AddMapping(".pcz", "application/octet-stream");
            AddMapping(".pdf", "application/pdf");
            AddMapping(".pfb", "application/octet-stream");
            AddMapping(".pfm", "application/octet-stream");
            AddMapping(".pfx", "application/x-pkcs12");
            AddMapping(".pgm", "image/x-portable-graymap");
            AddMapping(".pko", "application/vnd.ms-pki.pko");
            AddMapping(".pma", "application/x-perfmon");
            AddMapping(".pmc", "application/x-perfmon");
            AddMapping(".pml", "application/x-perfmon");
            AddMapping(".pmr", "application/x-perfmon");
            AddMapping(".pmw", "application/x-perfmon");
            AddMapping(".png", "image/png");
            AddMapping(".pnm", "image/x-portable-anymap");
            AddMapping(".pnz", "image/png");
            AddMapping(".pot", "application/vnd.ms-powerpoint");
            AddMapping(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12");
            AddMapping(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
            AddMapping(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12");
            AddMapping(".ppm", "image/x-portable-pixmap");
            AddMapping(".pps", "application/vnd.ms-powerpoint");
            AddMapping(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
            AddMapping(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            AddMapping(".ppt", "application/vnd.ms-powerpoint");
            AddMapping(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
            AddMapping(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            AddMapping(".prf", "application/pics-rules");
            AddMapping(".prm", "application/octet-stream");
            AddMapping(".prx", "application/octet-stream");
            AddMapping(".ps", "application/postscript");
            AddMapping(".psd", "application/octet-stream");
            AddMapping(".psm", "application/octet-stream");
            AddMapping(".psp", "application/octet-stream");
            AddMapping(".pub", "application/x-mspublisher");
            AddMapping(".qt", "video/quicktime");
            AddMapping(".qtl", "application/x-quicktimeplayer");
            AddMapping(".qxd", "application/octet-stream");
            AddMapping(".ra", "audio/x-pn-realaudio");
            AddMapping(".ram", "audio/x-pn-realaudio");
            AddMapping(".rar", "application/octet-stream");
            AddMapping(".ras", "image/x-cmu-raster");
            AddMapping(".rf", "image/vnd.rn-realflash");
            AddMapping(".rgb", "image/x-rgb");
            AddMapping(".rm", "application/vnd.rn-realmedia");
            AddMapping(".rmi", "audio/mid");
            AddMapping(".roff", "application/x-troff");
            AddMapping(".rpm", "audio/x-pn-realaudio-plugin");
            AddMapping(".rtf", "application/rtf");
            AddMapping(".rtx", "text/richtext");
            AddMapping(".scd", "application/x-msschedule");
            AddMapping(".sct", "text/scriptlet");
            AddMapping(".sea", "application/octet-stream");
            AddMapping(".setpay", "application/set-payment-initiation");
            AddMapping(".setreg", "application/set-registration-initiation");
            AddMapping(".sgml", "text/sgml");
            AddMapping(".sh", "application/x-sh");
            AddMapping(".shar", "application/x-shar");
            AddMapping(".sit", "application/x-stuffit");
            AddMapping(".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12");
            AddMapping(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
            AddMapping(".smd", "audio/x-smd");
            AddMapping(".smi", "application/octet-stream");
            AddMapping(".smx", "audio/x-smd");
            AddMapping(".smz", "audio/x-smd");
            AddMapping(".snd", "audio/basic");
            AddMapping(".snp", "application/octet-stream");
            AddMapping(".spc", "application/x-pkcs7-certificates");
            AddMapping(".spl", "application/futuresplash");
            AddMapping(".src", "application/x-wais-source");
            AddMapping(".ssm", "application/streamingmedia");
            AddMapping(".sst", "application/vnd.ms-pki.certstore");
            AddMapping(".stl", "application/vnd.ms-pki.stl");
            AddMapping(".sv4cpio", "application/x-sv4cpio");
            AddMapping(".sv4crc", "application/x-sv4crc");
            AddMapping(".swf", "application/x-shockwave-flash");
            AddMapping(".t", "application/x-troff");
            AddMapping(".tar", "application/x-tar");
            AddMapping(".tcl", "application/x-tcl");
            AddMapping(".tex", "application/x-tex");
            AddMapping(".texi", "application/x-texinfo");
            AddMapping(".texinfo", "application/x-texinfo");
            AddMapping(".tgz", "application/x-compressed");
            AddMapping(".thmx", "application/vnd.ms-officetheme");
            AddMapping(".thn", "application/octet-stream");
            AddMapping(".tif", "image/tiff");
            AddMapping(".tiff", "image/tiff");
            AddMapping(".toc", "application/octet-stream");
            AddMapping(".tr", "application/x-troff");
            AddMapping(".trm", "application/x-msterminal");
            AddMapping(".tsv", "text/tab-separated-values");
            AddMapping(".ttf", "application/octet-stream");
            AddMapping(".txt", "text/plain");
            AddMapping(".u32", "application/octet-stream");
            AddMapping(".uls", "text/iuls");
            AddMapping(".ustar", "application/x-ustar");
            AddMapping(".vbs", "text/vbscript");
            AddMapping(".vcf", "text/x-vcard");
            AddMapping(".vcs", "text/plain");
            AddMapping(".vdx", "application/vnd.ms-visio.viewer");
            AddMapping(".vml", "text/xml");
            AddMapping(".vsd", "application/vnd.visio");
            AddMapping(".vss", "application/vnd.visio");
            AddMapping(".vst", "application/vnd.visio");
            AddMapping(".vsto", "application/x-ms-vsto");
            AddMapping(".vsw", "application/vnd.visio");
            AddMapping(".vsx", "application/vnd.visio");
            AddMapping(".vtx", "application/vnd.visio");
            AddMapping(".wav", "audio/wav");
            AddMapping(".wax", "audio/x-ms-wax");
            AddMapping(".wbmp", "image/vnd.wap.wbmp");
            AddMapping(".wcm", "application/vnd.ms-works");
            AddMapping(".wdb", "application/vnd.ms-works");
            AddMapping(".wks", "application/vnd.ms-works");
            AddMapping(".wm", "video/x-ms-wm");
            AddMapping(".wma", "audio/x-ms-wma");
            AddMapping(".wmd", "application/x-ms-wmd");
            AddMapping(".wmf", "application/x-msmetafile");
            AddMapping(".wml", "text/vnd.wap.wml");
            AddMapping(".wmlc", "application/vnd.wap.wmlc");
            AddMapping(".wmls", "text/vnd.wap.wmlscript");
            AddMapping(".wmlsc", "application/vnd.wap.wmlscriptc");
            AddMapping(".wmp", "video/x-ms-wmp");
            AddMapping(".wmv", "video/x-ms-wmv");
            AddMapping(".wmx", "video/x-ms-wmx");
            AddMapping(".wmz", "application/x-ms-wmz");
            AddMapping(".wps", "application/vnd.ms-works");
            AddMapping(".wri", "application/x-mswrite");
            AddMapping(".wrl", "x-world/x-vrml");
            AddMapping(".wrz", "x-world/x-vrml");
            AddMapping(".wsdl", "text/xml");
            AddMapping(".wvx", "video/x-ms-wvx");
            AddMapping(".x", "application/directx");
            AddMapping(".xaf", "x-world/x-vrml");
            AddMapping(".xaml", "application/xaml+xml");
            AddMapping(".xap", "application/x-silverlight-app");
            AddMapping(".xbap", "application/x-ms-xbap");
            AddMapping(".xbm", "image/x-xbitmap");
            AddMapping(".xdr", "text/plain");
            AddMapping(".xla", "application/vnd.ms-excel");
            AddMapping(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
            AddMapping(".xlc", "application/vnd.ms-excel");
            AddMapping(".xlm", "application/vnd.ms-excel");
            AddMapping(".xls", "application/vnd.ms-excel");
            AddMapping(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
            AddMapping(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12");
            AddMapping(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            AddMapping(".xlt", "application/vnd.ms-excel");
            AddMapping(".xltm", "application/vnd.ms-excel.template.macroEnabled.12");
            AddMapping(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
            AddMapping(".xlw", "application/vnd.ms-excel");
            AddMapping(".xml", "text/xml");
            AddMapping(".xof", "x-world/x-vrml");
            AddMapping(".xpm", "image/x-xpixmap");
            AddMapping(".xps", "application/vnd.ms-xpsdocument");
            AddMapping(".xsd", "text/xml");
            AddMapping(".xsf", "text/xml");
            AddMapping(".xsl", "text/xml");
            AddMapping(".xslt", "text/xml");
            AddMapping(".xsn", "application/octet-stream");
            AddMapping(".xtp", "application/octet-stream");
            AddMapping(".xwd", "image/x-xwindowdump");
            AddMapping(".z", "application/x-compress");
            AddMapping(".zip", "application/x-zip-compressed");
        }
    }
}