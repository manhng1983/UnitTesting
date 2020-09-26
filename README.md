D:\manh83vn>md UnitTesting

D:\manh83vn>cd UnitTesting

D:\manh83vn\UnitTesting>git init
Initialized empty Git repository in D:/manh83vn/UnitTesting/.git/

D:\manh83vn\UnitTesting>notepad README.md

D:\manh83vn\UnitTesting>git add README.md

D:\manh83vn\UnitTesting>git remote add origin https://github.com/manhng1983/UnitTesting.git

D:\manh83vn\UnitTesting>git commit -m "first commit"
[master (root-commit) 49607c8] first commit
 1 file changed, 0 insertions(+), 0 deletions(-)
 create mode 100644 README.md

D:\manh83vn\UnitTesting>git branch -M master

D:\manh83vn\UnitTesting>git push -u origin master
Enumerating objects: 3, done.
Counting objects: 100% (3/3), done.
Writing objects: 100% (3/3), 208 bytes | 208.00 KiB/s, done.
Total 3 (delta 0), reused 0 (delta 0), pack-reused 0
To https://github.com/manhng1983/UnitTesting.git
 * [new branch]      master -> master
Branch 'master' set up to track remote branch 'master' from 'origin'.

D:\manh83vn\UnitTesting>git add *

D:\manh83vn\UnitTesting>git commit -m "second commit"
[master 56271d6] second commit
 590 files changed, 553190 insertions(+)

D:\manh83vn\UnitTesting>git push -u origin master
Enumerating objects: 707, done.
Counting objects: 100% (707/707), done.
Delta compression using up to 8 threads
Compressing objects: 100% (678/678), done.
Writing objects: 100% (706/706), 49.18 MiB | 5.08 MiB/s, done.
Total 706 (delta 280), reused 0 (delta 0), pack-reused 0
remote: Resolving deltas: 100% (280/280), done.
To https://github.com/manhng1983/UnitTesting.git
   49607c8..56271d6  master -> master
Branch 'master' set up to track remote branch 'master' from 'origin'.

D:\manh83vn\UnitTesting>
