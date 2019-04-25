# TagStore

## Store
* Item Store
  - Type
    - File (physical file)
    - Package (pack of multiple files)
    - Folder (just a node with no content)
  - ItemId (Guid)
  - Name
  - Tags
  
* Tags Store
  - Item
  - TagId (string)
  - Value (string,int,float,date, ...)
  - Virtual (true if this tag 'calculated' is from a parent item)
  
* TagInfo Store
  - TagId (string)
  - Type (string?)
  - Name (strings per locale)

## Relations
 * Items can be organized as a tree (maybe as multiple trees?)
 * Tags can have a TagInfo

## Search
* Get Item by ItemId (optional: include Path, include Tags, include TagTypes)
  - Path (calculated), Tags on this Item, TagTypes for each Tag  
* Get TagItems by TagId
  - TagId, Type, Name (for the current locale)
  
* Path-Navigation
  - Get parent item for path (cd ..)
  - List sub-items for Path (dir)
  - List sub paths for Path (dir)
  
* Find Item via Tag-List
  - Search for item having all specified Tags
    - Tag  operators (HAVE or HAVE NOT ?)
  - Search for item having all specified Tags and Values
    - Value operators (lt, gt, ???)
* Find available Tags by ItemId (for a navigation via Tags)
* Find available Tag-Values by ItemId (for a navigation via Tags)
