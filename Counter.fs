namespace Samplor

module Counter =
    open System
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Components
    open Avalonia.Layout
    open Avalonia.Media

    type MyMenu =
        { header: string
          icon: Image option
          items: MyMenu list }

    type State =
        { count: int
          menuItems: MyMenu list }


    let init =
        let menuItems = [ 
            { header = "Files"
              icon = None
              items = [
                  { header = "Open Files"
                    icon = None
                    items = [] }
                  { header = "Directory"
                    icon = None
                    items = [] } ]
            }
            { header = "Edit"
              icon = None
              items = [ 
                  { header = "Tools"
                    icon = None
                    items = [] }
                  { header = "Preferences"
                    icon = None
                    items = [] } ] 
            } 
         ]
        { count = 0
          menuItems = menuItems }

    type Msg =
        | Increment
        | Decrement
        | Reset

    let update (msg: Msg) (state: State): State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init

    let rec private menuItemTemplateStackPanel (menu: MyMenu) =
        StackPanel.create [
            StackPanel.children [
                MenuItem.create [ 
                    MenuItem.header menu.header
                    if menu.icon.IsSome then MenuItem.icon menu.icon.Value
                    MenuItem.dataItems menu.items
                    MenuItem.itemTemplate (DataTemplateView<MyMenu>.create menuItemTemplateStackPanel) 
                ]
            ]
        ]
    let rec private menuItemTemplateNoStackPanel (menu: MyMenu) =
        MenuItem.create [ 
            MenuItem.header menu.header
            if menu.icon.IsSome then MenuItem.icon menu.icon.Value
            MenuItem.dataItems menu.items
            MenuItem.itemTemplate (DataTemplateView<MyMenu>.create menuItemTemplateNoStackPanel) 
        ]

    let view (state: State) (dispatch) =
        Menu.create [
            Menu.viewItems [ 
                for menu in state.menuItems do
                    menuItemTemplateStackPanel menu 
                for menu in state.menuItems do
                    menuItemTemplateNoStackPanel menu 
            ] 
        ]
