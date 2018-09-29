modal-logic-calculator
======================


Building:

```
csc *.cs -r:/usr/lib/mono/4.0-api/System.Windows.Forms.dll
```
Calculates value of formula in modal algebra



Put a formula with connectives > (implication), & (conjunction), ~ (negation), # (existential modality) into 
the bottom narrow field. It can be of the form:
```
p>#(qju&#~p)
```

Into the left panel put description of the finite algebra. For example
```
1:1,2
2:3
3:
```
which means that in the considered algebra #{1}={1,2}, #{2}={3} and #{3}={} (modal operation is determined by the values on
atoms only, so if we have Boolean algebra P({1,2,3}), then it becomes modal algebra due to operation #).

Into the right panel put a valuation, for example:

```
p:1
qju:1,3
```

Remember that you must determine the number of atoms (if any is not listed on the left, it is valuated to the empty set).
