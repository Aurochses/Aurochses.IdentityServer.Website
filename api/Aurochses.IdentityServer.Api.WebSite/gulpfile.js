/// <binding BeforeBuild='default' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var _ = require("lodash"),
    gulp = require("gulp");

// APP
var app = {
    favicon: [
        "./favicon.ico"
    ]
};

// Favicon
gulp.task("copy-favicon", function (done) {
    _.forEach(app.favicon, function (file) {
        gulp.src(file)
            .pipe(gulp.dest("./wwwroot"));
    });

    done();
});

gulp.task("default", gulp.series("copy-favicon"));