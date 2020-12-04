# BettHelper

This is a little .Net-Console project to generate a forecast for the
results of the next matches in the German soccer Bundesliga.

The tool loads the results of past matches from [OpenLigaDB](https://www.openligadb.de/). It then
calculates weighted averages of home- and away-goals and uses the [Poison-distribution](https://en.wikipedia.org/wiki/Poisson_distribution)
to generate a forecast for goals and the final result of each upcoming match.

The results are written as a markdown-report which can then be transformed to e.g. PDF by using [Pandoc](https://pandoc.org/).