# ReadJEnc
ReadJEnc C#(.NET) File Character Code Type Automatic Determination Library

## Overview
C#(.NET Framework) Text file for automatic character code character recognition & reading library. 

This is a character code auto-identification function of my own grep tool (TresGrep / HNXgrep) cut out as a library of C #.
 * Compatible: .NET 2.0/3.5/4.0/4.5/4.6/4.7, .NET Core 1.0/1.1/2.0, .NET Standard 1.3/2.0

## Where to get it
 * NuGet
	* https://www.nuget.org/packages/ReadJEnc/
 * GitHub(Release)
	* https://github.com/hnx8/ReadJEnc/releases
	* download ReadJEnc_(yyyymmdd).zip 
 * Vector (Japanese only)
	* http://www.vector.co.jp/soft/winnt/util/se506899.html

## How does it work? (Japanese only)
 * https://github.com/hnx8/ReadJEnc/blob/master/ReadJEnc_Readme.txt

## Instructions
 * See the Example below.
```cs
	// Specify the file whose character encoding is to be determined by FileInfo object etc.
	void Example(System.IO.FileInfo file) 
	{

	    using (Hnx8.ReadJEnc.FileReader reader = new FileReader(file))
	    {
	        // Perform reading & detect encoding.
	        Hnx8.ReadJEnc.CharCode c = reader.Read(file);
	        // Get file-type name form the Name property. Get encoding object from the GetEncoding() method.
	        string name = c.Name;
	        Console.WriteLine("【" + name + "】" + file.Name);
	        System.Text.Encoding enc = c.GetEncoding(); 
	        // The actual readout character string can be obtained from the Text property.
	        // Note : For non-text-files, null is returned.
	        string text = reader.Text;
	        // From the type of return value, you can grasp the rough classification of the file.
	        if (c is CharCode.Text) 
	        {
	            Console.WriteLine("-------------------------------------");
	            Console.WriteLine(text);
	        }
	    }
	}
```
 * Also see another Code sample `Hnx8.ReadJEnc.WinForm.Sample\SampleForm.cs`.
 * In addition, a sample that judges the character code from the contents of the byte array is posted in the atmarkIT.
	* @ IT "[. NET TIPS: How to estimate character encoding using ReadJEnc? (Japanese only)](http://www.atmarkit.co.jp/ait/articles/1501/20/news073.html)」 
 * For more information, see [ReadJEnc_Readme.txt(Japanese only)](https://github.com/hnx8/ReadJEnc/blob/master/ReadJEnc_Readme.txt).

## Other Supplement
The main part of the source code is briefly explained in the following blog article. 
* [Automatic determination of text file character code (2014 version) (BLOG article: 2014.08.24 in Japanese only)] (http://d.hatena.ne.jp/hnx8/20140824/1408844344)

## Library incorporation example
 * TresGrep 
	* http://hp.vector.co.jp/authors/VA055804/TresGrep/
 * HNXgrep 
	* http://hp.vector.co.jp/authors/VA055804/HNXgrep/
 * Etc.

## Contact
If you have any problems, doubts or impressions about this library, please give feedback to the appropriate articles of the author BLOG.
* [hnx8 development room (author's BLOG) category "ReadJEnc" article list (Japanese only)](http://d.hatena.ne.jp/hnx8/archive?word=%2A%5BReadJEnc%5D)

[Issues](https://github.com/hnx8/ReadJEnc/issues),[Pull requests](https://github.com/hnx8/ReadJEnc/pulls) will also accept contact.

## License
 - [MIT License](https://github.com/hnx8/ReadJEnc/blob/master/LICENSE)
