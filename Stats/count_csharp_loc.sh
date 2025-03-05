# counts lines of code in all C# files
git ls-files .. | grep '\.cs' | xargs wc -l
