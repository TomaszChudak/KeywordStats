function StatsVM() {
    var self = this;

    self.requestPageUrl = ko.observable();
    self.pageUrl = ko.observable();
    self.keywordStats = ko.observableArray();
    self.loading = ko.observable();
    self.error = ko.observable();

    self.getStats = function() {
        self.loading(true);
        self.error(null);

        $.ajax({
            type: "GET",
            dataType: "json",
            url: "https://localhost:44344/api/stats",

            data: {
                url: self.requestPageUrl(),
            },
            success: function(pageStatsJson) {
                self.parseResults(pageStatsJson);
            },
            error: function(aa, bb, cc) {
                console.log("error");
            },
            complete: function() {
                self.loading(false);
            }
        });
    };

    self.parseResults = function(pageStatsJson) {
        if (pageStatsJson.errorDesc) {
            self.error(pageStatsJson.errorDesc);
            return;
        }
        self.pageUrl(pageStatsJson.pageUrl);
        self.keywordStats([]);
        $.each(pageStatsJson.keywordStats,
            function(index, element) {
                self.keywordStats.push(new KeywordStatVM(element));
            });

        self.keywordStats.sort(function(a, b) {
            return a.keyword.toLowerCase() > b.keyword.toLowerCase() ? 1 : -1;
        });
    };
}

function KeywordStatVM(keywordStats) {
    var self = this;

    self.keyword = keywordStats.keyword;
    self.count = keywordStats.count;
}