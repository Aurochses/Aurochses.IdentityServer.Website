/// <binding BeforeBuild='default' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var _ = require("lodash"),
    gulp = require("gulp"),
    sass = require("gulp-sass");

// APP
var app = {
    favicon: [
        "./favicon.ico"
    ],
    css: [
        "./Content/**/*.scss"
    ],
    fonts: [
        "./node_modules/bootstrap-sass/assets/fonts/bootstrap/*.*",
        "./node_modules/font-awesome/fonts/*.*"
    ],
    images: [
        "./Images/**"
    ],
    js: [
        { src: "./Scripts/**/*.js", dest: "./wwwroot/js" },
        { src: "./node_modules/bootstrap-sass/assets/javascripts/bootstrap.js", dest: "./wwwroot/js/bootstrap" },
        { src: "./node_modules/jquery/dist/jquery.js", dest: "./wwwroot/js/jquery" },
        { src: "./node_modules/jquery-validation/dist/jquery.validate.js", dest: "./wwwroot/js/jquery-validation" },
        { src: "./node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js", dest: "./wwwroot/js/jquery-validation-unobtrusive" }
    ]
};

// Favicon
gulp.task("copy-favicon", function () {
    _.forEach(app.favicon, function (file) {
        gulp.src(file)
            .pipe(gulp.dest("./wwwroot"));
    });
});

// Css
gulp.task("copy-css", function () {
    _.forEach(app.css, function (file) {
        gulp.src(file)
            .pipe(sass())
            .pipe(gulp.dest("./wwwroot/css"));
    });
});

// Fonts
gulp.task("copy-fonts", function () {
    _.forEach(app.fonts, function (file) {
        gulp.src(file)
            .pipe(gulp.dest("./wwwroot/fonts"));
    });
});

// Images
gulp.task("copy-images", function () {
    _.forEach(app.images, function (file) {
        gulp.src(file)
            .pipe(gulp.dest("./wwwroot/images"));
    });
});

// JavaScript
gulp.task("copy-js", function () {
    _.forEach(app.js, function (file) {
        gulp.src(file.src)
            .pipe(gulp.dest(file.dest));
    });
});

gulp.task("default", ["copy-favicon", "copy-css", "copy-fonts", "copy-images", "copy-js"]);