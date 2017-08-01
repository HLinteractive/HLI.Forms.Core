![HL Interactive](https://www.dropbox.com/s/fdyzvkso9zs9ndf/HLi.Signature.DVDs.jpg?dl=1)
> Copyright © HL Interactive 2015, Thomas Hagström,
> Horisontvägen 85, Stockholm, Sweden

- [HLI.Forms.Core](#hliformscore)
  * [Usage](#usage)
    + [Services](#services)
    + [Converters](#converters)
    + [Extensions](#extensions)
    + [Controls](#controls)
      - [HliBarChart](#hlibarchart)
      - [HliComboBox](#hlicombobox)
      - [HliFeedbackView](#hlifeedbackview)
      - [HliImageButton](#hliimagebutton)
      - [HliItemsView](#hliitemsview)
      - [HliLinkButton](#hlilinkbutton)
      - [HliPlaceholderView](#hliplaceholderview)
      - [HliOrientatedView](#hliorientatedview)
  * [Delivery & Deployment](#delivery---deployment)
  * [Dependencies](#dependencies)
    + [NuGet Package Generation](#nuget-package-generation)
  * [Solution File Structure](#solution-file-structure)
  * [Changes and backward compatibility](#changes-and-backward-compatibility)

# HLI.Forms.Core #
Xamarin.Forms boilerplate functions to facilitate project creation

[![NuGet](https://img.shields.io/nuget/v/HLI.Forms.Core.svg)
![Downloads](https://img.shields.io/nuget/dt/HLI.Forms.Core.svg)](https://preview.nuget.org/packages/HLI.Forms.Core/)
![Build Status VSTS](https://nodessoft.visualstudio.com/_apis/public/build/definitions/7452d5a3-4e17-4d49-a0df-4f3b5961c31b/23/badge)

## Usage
### Services
`AppService` and `SpeechService`
### Converters
Common converters such as `BoolToInvertedConverter`and `BytesToImageSourceConverter`
### Extensions
Common extension methods. You should see this pop up in intellisense on `Application`, `Object`, `Page`, `String`, `View` etc.
### Controls
Pure Xamarin.Forms Views with no platform specific implentation.  
Import the namespace to start using them:
```xaml
	<ContentPage 
	xmlns:hli="clr-namespace:HLI.Forms.Core.Controls;assembly=HLI.Forms.Core">
	</ContentPage> 
```
#### HliBarChart
Creates a simple bar chart from **`ItemsSource`** using **`ValuePath`** and **`LabelPath`** to determine object members to display.

![Screenshot](https://github.com/HLinteractive/HLI.Forms.Core/blob/master/Screenshots/HliBarChart.PNG?raw=true)

```xaml
    <hli:HliBarChart ItemsSource="{Binding CostPerWeekItems}" ValuePath="Cost" LabelPath="Week" IsPercent="True" BarScale="2" />
```

#### HliComboBox
Allows binding a **`Picker`** to an **`ItemsSource`** of objects and using **`DisplayMemberpath`** with **`SelectedValuePath`** 
Also supports binding the selected object using **`SelectedItem`**.  
Based on **[bindable picker written by Simon Villiard](https://forums.xamarin.com/discussion/30801/xamarin-forms-bindable-picker)**

![Screenshot](https://github.com/HLinteractive/HLI.Forms.Core/blob/master/Screenshots/HliComboBox.GIF?raw=true)

```xaml
    <hli:HliComboBox ItemsSource="ItemsSource="{Binding Countries}"" DisplayMemberpath="Name" SelectedItem="{Binding SelectedItem, Mode=TwoWay}" />
```

#### HliFeedbackView
Displays feedback to the user when a **`HliFeedbackMessage`** is published through **`MessagingCenter`** using hte key **`Constants.FeedbackKeys.Message`**

**XAML**

```xaml
    <hli:HliFeedbackView />
```

**Code**

```csharp
    MessagingCenter.Send(new HliFeedbackMessage(HliFeedbackMessage.FeedbackType.Error, "We're sorry!"), Constants.FeedbackKeys.Message);
```

#### HliImageButton
Display a button with **`Image`** content optionally using an **`ImageConverter`**

    <hli:HliImageButton Image="myImage.png" />

#### HliItemsView
A simple listview for a smaller collection when **`ListView`** is not required.  
Binds **`ItemsSource`** using **`ItemTemplate`**

```xaml
    <hli:HliItemsView ItemsSource="MyItems">
    	<hli:HliItemsView.ItemTemplate>
			<DataTemplate>
    			<!-- View content -->
			</DataTemplate>
    	</hli:HliItemsView.ItemTemplate>
    </hli:HliItemsView>
```

#### HliLinkButton
Displays **`Text`** as a simple clickable link. Supports **`Command`** and **`ClickedEvent`**

#### HliPlaceholderView
A view that displays **`UnfocusedView`** when unfocused and **`FocusedPage`** when the user is editing using **`Navigation.PushModalAsync`**.

The `FocusedPage` has a **`CloseButton`** you can customize.

here is also a **`OnClosed`** event and **`ClosedCommand`** you can subscribe to.

![Screenshot](https://github.com/HLinteractive/HLI.Forms.Core/blob/master/Screenshots/HliPlaceholderlView.GIF?raw=true)


```xaml
        <hli:HliPlaceholderView>
            <hli:HliPlaceholderView.UnfocusedView>
                <Label Text="Read more!"></Label>
            </hli:HliPlaceholderView.UnfocusedView>
            
            <hli:HliPlaceholderView.FocusedPage>
                <ContentPage BackgroundColor="White" Padding="20">
                    <Grid BackgroundColor="Gray" Padding="20" HorizontalOptions="Fill" VerticalOptions="Fill">
                        <ScrollView HorizontalOptions="Fill" VerticalOptions="Fill">
                            <Label TextColor="White" Margin="10,0,0,0"
                               HorizontalOptions="Fill" VerticalOptions="Fill"
                               Text="Odio velit duis! Habitasse mauris scelerisque vel quis, in in? Tristique nisi auctor pulvinar non pellentesque quis nec! Adipiscing urna egestas massa et, enim! Nisi et, pulvinar tristique integer nascetur! Ac cum. Aliquet, dictumst scelerisque. Eu! Ultrices rhoncus ut nec etiam vut, diam placerat sed? Integer ultrices amet sed scelerisque et. Duis adipiscing tincidunt tincidunt turpis, quis diam, placerat quis! In magnis ac in, sed aliquam, sit eu mus habitasse dictumst mattis! Ac nec turpis scelerisque! Velit et. Mid et! Lectus mattis duis porta! Augue risus et augue, diam amet. Ut pellentesque, porta, odio! Adipiscing mauris, sagittis a a augue porttitor, enim pulvinar dapibus? Aenean amet tincidunt et habitasse montes aenean. Scelerisque! Etiam natoque cras duis amet proin lectus in."
                               LineBreakMode="WordWrap">
                            </Label>
                        </ScrollView>
                    </Grid>
                </ContentPage>
            </hli:HliPlaceholderView.FocusedPage>
        </hli:HliPlaceholderView>
```

#### HliOrientatedView
 A view that respons to orientation changes and either display the **`PortraitContent`** or **`LandscapeContent`**

## Delivery & Deployment
Download the nuget package through Package Manager Console:

> install-package HLI.Forms.Core

## Dependencies
* **Projects**
* **Packages**
	* HLI.Core
	* System.Linq.Expressions 
	* System.Linq.Queryable
	* Xamarin.Forms

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
* VS2017 CsProj NetStandard 1.4