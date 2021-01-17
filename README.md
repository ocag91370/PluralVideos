# PluralVideos
 
Command line tools for decrypting Pluralsight videos downloaded from the Pluralsight Offline Player and downloading decrypted videos

## Prerequisites

- This tools requires .Net Core 3.1

## Installing
- Download the latest binary from [here](https://github.com/dodoma700/PluralVideos/releases).

## Usage

### PluralVideos
This tool can decrypts and downloads courses that you are subscribed to in Pluralsight. To download you need to login using `pluralvideos auth --login`

### Verbs
```diff
  auth        Authenticates the app to pluralsight
  download    Downloads courses from pluralsight
  decrypt     Decrypts videos downloaded by pluralsight app
```

#### Auth Options
```diff
  --login      Login the App to Pluralsight
  --logout     Logout the App from Pluralsight
```

#### Download Options
You can only download videos that your subscription allows
 ```diff
  --out        Required. Output folder path
  --course     Required. Course to download
  --module     Video clip to download
  --clip       Video clip to download
  --list       List course without downloading
  --timeout    (Default: 15) Timeout period for video download in seconds
```
 
The `--course` flag is the string on the url table of content. The Url ***/library/courses/linq-fundamentals-csharp-6/table-of-contents*** the course flag would be ***linq-fundamentals-csharp-6***

To download a module or a single video you need to get the course id or the clip id. To get these ids run the `--list` flag which will list the all modules with their ids

**Examples:**

- ***List course content***
```diff
pluralvideos download --out <outputPath> --course linq-fundamentals-csharp-6 --list
```
- ***Download a full course***
```diff
pluralvideos download --out <outputPath> --course linq-fundamentals-csharp-6
```
- ***Download a single video***
```diff
pluralvideos download --out <outputPath> --course linq-fundamentals-csharp-6 --clip 97619f0d-5618-4a53-8dc8-08fa981883fc
```
- ***Download a single module***
```diff
pluralvideos download --out <outputPath> --course linq-fundamentals-csharp-6 --module 97619f0d-5618-4a53-8dc8-08fa981883fc
```
	
### Decrypt options
Decrypts video already downloaded by pluralsight app
```diff
Flags
	--out        Required. Output folder path
	--db         Database file path
	--course     Course folder path
	--trans      Create subtitle file along with the video
	--rm         Remove encrypted folder after decryption
	--help       Display this help screen.
```
The database flag `--db` defaults to the default location where the pluralsight store the video so is the courses folder `--course`.

To remove videos after decrypting use `--rm` flag. To include transcript use `--trans`

**Examples:**

- ***Decrypt videos from the default location with transcript***
```diff
pluralvideos --out <outputPath> --trans
```

**Notes:**
- Do not  remove the course from the Pluralsight Offine Player before decrypting. You can add `---delete` checkbox to remove the course after the course decrypted.
- Some courses don't have subtitles.

## Author

- Dodoma

## Copyright ©

- This software is freeware and open source and is only intended for personal or educational use
``` diff
-  Pluralsight Terms of Use does not allow downloading/storing of the video. https://www.pluralsight.com/terms
``` 
