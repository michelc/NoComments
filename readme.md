# NoComments

[![Build status](https://ci.appveyor.com/api/projects/status/r1e24cfna8fy6gmv)](https://ci.appveyor.com/project/michelc/nocomments)

Remove comments from SQL commands. No regex.

I tried to adapt some snippets with [regex](http://stackoverflow.com/questions/3524317/regex-to-strip-line-comments-from-c-sharp), but results weren't successful.


## Before

```SQL
/* Demo */
SELECT *
     , 'a -- false comment' AS Test1
     , 'another /* false comment */' AS Test2
FROM   OneTable -- Or Customers?
WHERE  Caption LIKE 'C%' /* Customers! */
-- AND    ID <> 3
```


## After

```SQL
SELECT *
     , 'a -- false comment' AS Test1
     , 'another /* false comment */' AS Test2
FROM   OneTable
WHERE  Caption LIKE 'C%'
```


## Regex method

If regular expressions are your preference, I finally found:

* Larry's explanations : [Use Regular Expressions to Clean SQL Statements](http://larrysteinle.com/2011/02/09/use-regular-expressions-to-clean-sql-statements/)
* Wangxn's perfect solution : http://space.scmlife.com/blog-8945-626.html
