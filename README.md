# Elgindy-VTT-to-SRT-Converter
A tool for converting Web Video Text Tracks Format (WebVTT) subtitle to srt one. As most of video players support srt subtitles and can't open vtt subtitles, We should convert vtt to srt or sub subtitles but it's not easy to do that.

Web Video Text Tracks Format (WebVTT)( https://developer.mozilla.org/en-US/docs/Web/API/WebVTT_API ) is a modern subtitle format used for online video subtitles. This tool converts many text-based subtitle formats to vtt.
WebVTT is also used widely, especially for e-Learning localization and multimedia applications, since it works particularly well with HTML5-based platforms.

The SubRip text format – commonly called SRT – was initially developed as part of a program that extracts captions and subtitles from media files. This SRT text format was notable for its simplicity and ease-of-use, especially when compared to other formats available at the time, many of which used XML-based code.
 
You can select multiple files or a folder to convert a batch of subtitles at once.

VTT vs. SRT 
1. Caption numbers. VTT files can have caption numbers, but they're not actually necessary, as you can see in the file above. SRTs must have them.
2. Time-code format. SRT separates seconds from milliseconds with a comma. VTT uses a period instead (see the time-code in yellow above). Also, no time-code hours are required in VTT files, though they’re almost always provided.
3. Metadata. WebVTT files can have metadata, and in fact, some is required, in particular having WEBVTT in the first line of the file. The VTT screenshot above has the full header highlighted (it includes file type and language), as well as a metadata note in the body. SRT can’t support metadata.
4. Formatting options. WebVTT has very robust features, including font, color and text formatting, and placement. Initially SRT couldn’t support any formatting, but it’s been upgraded to support basic text formats (bold, italic, underline) and placement. However, it doesn’t have nearly the same capabilities as VTT.

Requirements:
.Net framework 4.5 && Win 7 SP1, Win 8, Win 8.1, Win 10, Win Server
