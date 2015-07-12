
var symbol = $('#symbol').attr('value');
$(document).ready(function() {

    $("#symbol-search input").autocomplete({
        source: "/Ajax/GetMatchingCompanies",
    }).data("autocomplete")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<a>" + item.company + "<strong>" + item.symbol + "</strong></a>")
            .appendTo(ul);
    };

    $(".dialog").click(function() {
        

        $.ajax({
            url: $(this).attr('href'),
            success: function (data) {
                var dialogue = $("#dialog").dialog({
                    modal: true,
                    width: 'auto'
                });
                dialogue.html(data);
                dialogue.dialog('open');
            }
        });
        return false;
    });

    // Massive massive hack. Sort this out
    if (symbol != '') {
        $.getJSON('/api/ChartResource/Get/?symbol=' + symbol, function(data) {

            // split the data set into ohlc and volume
            var ohlc = [],
                volume = [],
                forceIndexThirteenDay = [],
                upperBollingerBandTwoDeviation = [],
                lowerBollingerBandTwoDeviation = [],
                movingAverage20Day = [],
                macd = [],
                macdSignalLine = [],
                macdHistogram = [],
                dataLength = data.Data.length;

            for (i = 0; i < dataLength; i++) {
                ohlc.push([
                    data.Data[i][0], // the date
                    data.Data[i][1], // open
                    data.Data[i][2], // high
                    data.Data[i][3], // low
                    data.Data[i][4] // close
                ]);

                volume.push([
                    data.Data[i][0], // the date
                    data.Data[i][5] // the volume
                ]);

                forceIndexThirteenDay.push([
                    data.Data[i][0],
                    data.Data[i][6]
                ]);

                upperBollingerBandTwoDeviation.push([
                    data.Data[i][0],
                    data.Data[i][7]
                ]);

                lowerBollingerBandTwoDeviation.push([
                    data.Data[i][0],
                    data.Data[i][8]
                ]);

                macd.push([
                    data.Data[i][0],
                    data.Data[i][9]
                ]);

                macdSignalLine.push([
                    data.Data[i][0],
                    data.Data[i][10]
                ]);

                macdHistogram.push([
                    data.Data[i][0],
                    data.Data[i][11]
                ]);

                movingAverage20Day.push([
                    data.Data[i][0],
                    data.Data[i][12]
                ]);
            }

            // set the allowed units for data grouping
            var groupingUnits = [[
                    'week', // unit name
                    [1]                             // allowed multiples
                ], [
                    'month',
                    [1, 2, 3, 4, 6]
                ]];

            // create the chart
            $('#container').highcharts('StockChart', {
                rangeSelector: { inputEnabled: $('#container').width() > 480, selected: 1 },

                series: [
                    { type: 'candlestick', name: symbol, data: ohlc },
                    { name: 'Upper Bollinger Band 2 Deviation', data: upperBollingerBandTwoDeviation, color: '#FF0000' },
                    { name: 'Lower Bollinger Band 2 Deviation', data: lowerBollingerBandTwoDeviation, color: '#FF0000' },
                    { type: 'column', name: 'Volume', data: volume, yAxis: 1 },
                    { name: 'MACD', data: macd, yAxis: 2 },
                    { name: 'MACD Signal Line', data: macdSignalLine, yAxis: 2 },
                    { name: 'MACD Histogram', type: 'column', data: macdHistogram, yAxis: 2 },
                    { name: 'Force (13 Day)', data: forceIndexThirteenDay, yAxis: 3 },
                    { name: 'MA (20)', data: movingAverage20Day, color: '#000000' },
                ],

                plotOptions: {
                    candlestick: {
                        color: 'red',
                        upColor: 'green'
                    }
                },

                yAxis: [{
                        labels: {
                            align: 'right',
                            x: -3
                        },
                        title: {
                            text: 'OHLC'
                        },
                        height: '35%',
                        lineWidth: 2
                    },
                    {
                        labels: {
                            align: 'right',
                            x: -3
                        },
                        title: {
                            text: 'Volume'
                        },
                        top: '37%',
                        height: '8%',
                        offset: 0,
                        lineWidth: 2
                    },
                    {
                        labels: {
                            align: 'right',
                            x: -3
                        },
                        title: {
                            text: 'MACD'
                        },
                        top: '47%',
                        height: '13%',
                        offset: 0,
                        lineWidth: 2
                    },
                    {
                        labels: {
                            align: 'right',
                            x: -3
                        },
                        title: {
                            text: 'Force 13 Day'
                        },
                        top: '62%',
                        height: '20%',
                        offset: 0,
                        lineWidth: 2
                    }
                ]
            });
        });
    }
});