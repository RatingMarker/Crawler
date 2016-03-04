$locs = "src/Crawler.ConsoleApp", "src/Crawler.Workflow", "test/Crawler.UnitTest"

foreach ($loc in $locs) {
	cd $loc
	dnu restore
	cd ../..
}