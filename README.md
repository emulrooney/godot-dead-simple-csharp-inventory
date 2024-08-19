# EmuInventory

Very simple implementation of an inventory system for the [Godot](https://godotengine.org/) engine (using C#), v4.3.

This is fairly rudimentary: to use this, add an `InventoryContainer`, give it some child container nodes (and assign them to the `contentHolders` property for the
container), then add a set of `InventorySlot` to each of those holders. Add a `InventorySlotItem` to any `InventorySlot` that needs to be populated,
and you can drag and drop it between slots.

The `InventoryContainer` will emit a `Godot.Collections.Array` with the contents every time they change (even if it's just an order).
The slots will emit signals when they're populated or emptied out.

## What's missing?

* Would be useful to illustrate how it works with a simple demo game to allow picking stuff up, persisting an inventory, opening/closing containers...etc
* Items that occupy more than 1x1 slots (think [Deus Ex](https://www.google.com/search?sca_esv=41b22d2abb3efe70&sca_upv=1&q=deus+ex+1+inventory+screenshot&udm=2&fbs=AEQNm0AeMNWKf4PpcKMI-eSa16lJoRPMIuyspCxWO6iZW9F1Ns6EVsgc0W_0xN47PHaanAEtg26fpfc9gg2y1-ZsywNNidIzOA0khSyMN51n7r3LlDC9M1NYStuTRDcBUYQ58dKt-Q6SigUS4Yne5yDHLg0vPBr98Nz98twIaNcnWiKaD4QuEh93Q53sB-UkWP9OcfO5KeatY98HR7cDW9ZTjFpZV7kJtA&sa=X&ved=2ahUKEwjIy5rW2P-HAxU-g4kEHW9CHO4QtKgLegQIERAB&biw=1203&bih=698&dpr=1.88#vhid=jkImU1KkLrrVDM&vssid=mosaic))
* Items that have different quantities while remaining in one tile
* Interacting with items in any way (ie double click to use, right click for more actions... etc)
* Removing things from your inventory (trash can icon like [Stardew Valley](https://www.google.com/search?client=firefox-b-d&sca_esv=41b22d2abb3efe70&sca_upv=1&q=stardew+valley+inventory&udm=2&fbs=AEQNm0DPvcmG_nCbmwtBO9j6YBzM68ZanC7g01Skprhw5JoufVCiMv-hxC44jt6JduRQysBab-bgQXjPraaWFXMvOy8Kr1OAG3K-aj3De4zf3-LxKtkBtWaSCp743evHzhY6J0rIQUCXki65vOxhV0cGJtj0S1dF8YREnKrWtJctBkTv8-bs83YpB7p3IMTdYvjisDEty1xSxeLS4B_TKFXUiCrenmEMcA&sa=X&ved=2ahUKEwjP6o2u2v-HAxWypIkEHZGKIBYQtKgLegQIExAB&biw=1203&bih=698&dpr=1.88)? click outside the window to drop?)

## Bugs & sloppy design
* Right now the item *has* to have a TextureRect... this sucks, it should be more generic.
* Mouseover/mouseout signals should pause when you're dragging, so that content from the actively dragged item stays visible (instead of literally only tracking exactly where your mouse pointer is)

## Credits

* Food icons: https://henrysoftware.itch.io/pixel-food
