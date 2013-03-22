RandomizeArray
==============

Builds a numeric array of n and randomizes the values.

Arguments: [ [amount] method ]

Methods:

###RandomizeBySort (slow)###
 
   This method produces more random results, but is slower and less scalable. 
   It takes an ordered array of numbers, adds each item as a key in a Dictionary with a randoom number as it's value, then finally sorts the Dictionary by value.
 
   Performance:
   - 10k = 10ms
   - 1m  = 514ms
   - 10m = 7s 200ms


### ShufflePile (fast)###
 
   This method produces less random results, but is faster and more scalable.
   It copies the ordered array of numbers into a generic list (the pile). While the pile count is > 0 it "pops" random entries from the pile and assigns them sequentially to the main array.
   
   Performance:
   - 10k = 2ms
   - 1m  = 173ms
   - 10m = 2s 400ms
