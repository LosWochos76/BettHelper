# BettHelper

This is a little .Net-Console project to generate a forecast for the
results of the next matches in the German soccer Bundesliga.

The tool loads the results of past matches from [OpenLigaDB](https://www.openligadb.de/),
calculates weighted averages of home- and away-goals and uses the Posion-distribution 
to generate a forecast.

The results are written as a markdown-report.