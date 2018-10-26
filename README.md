# FileContentsSorter

Generator

  -l, --lines    Required. Lines to generate

  -f, --file     Output file path
  
  -b, --batch    Batch size (count of items to generate in one step)
  
Example: 
dotnet run -c release -l 2000000000 --file "d:\tmp\big"
  
Sorter 

  -s, --source    Required. Source file with the contents to sort

  -o, --output    File to save sorting results in

  -t, --temp      Directory to save temporary files in

  -b, --batch     Batch size (count of items to sort in one step)
  
Example: 
dotnet run -c release -s "D:\tmp\big" -t "D:\tmp\b" -o "D:\tmp\big.res" -b 33000000
