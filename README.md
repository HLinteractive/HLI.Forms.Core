![HL Interactive](https://www.dropbox.com/s/fdyzvkso9zs9ndf/HLi.Signature.DVDs.jpg?dl=1)
> Copyright © HL Interactive 2015, Thomas Hagström,
> Horisontvägen 85, Stockholm, Sweden

# <a name="hlidata"></a>HLI.Forms.Core #
Xamarin.Forms boilerplate functions to facilitate project creation

## Usage
### Services
`AppService` and `SpeechService`
### Converters
Common converters such as `BoolToInvertedConverter`and `BytesToImageSourceConverter`
### Extensions
Common extension methods. You should see this pop up in intellisense on `Application`, `Object`, `Page`, `String`, `View` etc.
### Controls
#### HliBarChart
Creates a simple bar chart from **`ItemsSource`** using **`ValuePath`** and **`LabelPath`** to determine object members to display.

    <hli:HliBarChart ItemsSource="{Binding CostPerWeekItems}" ValuePath="Cost" LabelPath="Week" />

#### HliComboBox
Allows binding a **`Picker`** to an **`ItemsSource`** of objects and using **`DisplayMemberpath`** with **`SelectedValuePath`** 
Based on **[bindable picker written by Simon Villiard](https://forums.xamarin.com/discussion/30801/xamarin-forms-bindable-picker)**

    <hli:HliComboBox ItemsSource="CountriesCollection" DisplayMemberpath="Name" SelectedValuePath="IsoCode" />

#### HliFeedbackView
Displays feedback to the user when a **`HliFeedbackMessage`** is published through **`MessagingCenter`** using hte key **`Constants.FeedbackKeys.Message`**

**XAML**

    <hli:HliFeedbackView />

**Code**

    MessagingCenter.Send(new HliFeedbackMessage(HliFeedbackMessage.FeedbackType.Error, "We're sorry!"), Constants.FeedbackKeys.Message);

#### HliImageButton
Display a button with **`Image`** content optionally using an **`ImageConverter`**

    <hli:HliImageButton Image="myImage.png" />

#### HliItemsView
A simple listview for a smaller collection when **`ListView`** is not required.  
Binds **`ItemsSource`** using **`ItemTemplate`**

    <hli:HliItemsView ItemsSource="MyItems">
    	<hli:HliItemsView.ItemTemplate>
    		<!-- View content -->
    	</hli:HliItemsView.ItemTemplate>
    </hli:HliItemsView>

#### HliLinkButton
Displays **`Text`** as a simple clickable link. Supports **`Command`** and **`ClickedEvent`**

#### HliPlaceholderView
A view that displays **`PlaceholderView`** when unfocused and **`ModalPage`** when the user is editing using **`Navigation.PushModalAsync`**.

    <hli:HliPlaceholderView ItemsSource="MyItems">
    	<hli:HliPlaceholderView.PlaceholderView>
    		<!-- Unfocused content -->
    	</hli:HliPlaceholderView.PlaceholderView>
		<!-- Focused content -->
    </hli:HliPlaceholderView>

#### HliOrientatedView
 A view that respons to orientation changes and either display the **`PortraitContent`** or **`LandscapeContent`**

## Delivery & Deployment
Download the nuget package through Package Manager Console:

> install-package HLI.Forms.Core

## Dependencies
* **Projects**
* **Packages**
	* HLI.Core

### NuGet Package Generation
The project is configured to automatically generate a ***.nupkg** upon build with **`dotnet cli`**.

## Solution File Structure

* **HLI.Forms.Core** - solution root folder
	* **HLI.Forms.Core**  - main project
		* **Controls** - Xamarin.Forms `Views` (above)
		* **Converters** - see above
		* **Extensions** - see above
		* **Interfaces**
		* **Models** - HLi models specific to project
		* **Resources** - Colors, localization, GFX etc
		* **Services** - see above

## Changes and backward compatibility
