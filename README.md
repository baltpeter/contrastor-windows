# Contrastor for Windows

> A simple color contrast ratio checker that lives in your Windows system tray.

![Using Contrastor to verify the color contrast ratios of a design.](https://cdn.baltpeter.io/img/contrastor-windows-demo.gif)

Checking color contrast ratios is an integral part of making sure your designs are accessible. The WCAG defines a number of [contrast levels](https://www.w3.org/TR/WCAG20-TECHS/G18.html) between two colors that tell you how easy it is to read the text when one of the colors is the foreground and the other the background color.  
While there are plenty of online tools available that let you enter the color codes and spit out the contrast ratio, Contrastor aims to make this process even easier. It lives right in your Windows system tray. After opening it, you can simply pick the foreground and background color from your screen using the eyedroppers. It will instantly tell you the contrast ratio. No nonsense, just a straightforward answer to the question "Do these two colors work together?"

If you are using macOS, check out [Contrast](https://usecontrast.com/), the app that inspired Constrator.

## The contrast levels

These are the possible contrast levels:

* **AAA**: The ideal contrast level. For that you need a score of at least `7.00`. For reference: Black (`#000000`) on white (`#FFFFFF`) has a contrast ratio of `21.00`.
* **AA**: Often, you won't be able to meet the contrast requirements for the best score. Colors with this level are still readable. For AA, you need a score of at least `4.5`.
* **AA\***: The last acceptable level, officially called **AA Large**. This score is only OK for a font size of at least `18pt`. The score here needs to be `3.0` or higher.
* **FAIL**: Anything below that. You shouldn't use this color combination.

For a more in-depth overview, have a look at [Contrast's great guide](https://usecontrast.com/guide).

## Installation

Contrastor is a portable application that doesn't need an installer. Simply grab the ZIP download of the [latest release](https://github.com/baltpeter/contrastor-windows/releases) and extract it somewhere. You can then simply launch Contrastor through the `Contrastor.exe`.

### Starting Contrastor automatically when you login

Just create a shortcut to Constrator in the [Startup folder](https://support.microsoft.com/en-us/help/4026268/windows-10-change-startup-apps) (Win + R -> `shell:startup`).

## License

Contrastor is licensed under the MIT license, see the [`LICENSE` file](LICENSE) for details. Pull requests are welcome!