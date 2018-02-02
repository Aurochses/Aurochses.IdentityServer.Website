/// <binding BeforeBuild='default' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var _ = require("lodash"),
    gulp = require("gulp"),
    gulpif = require("gulp-if"),
    rename = require("gulp-rename"),
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
        "./node_modules/font-awesome/fonts/*.*"
    ],
    images: [
        "./Images/**"
    ],
    js: [
        { src: "./bower_components/jquery-validation-unobtrusive-bootstrap/dist/unobtrusive-bootstrap.js", dest: "./wwwroot/js/jquery-validation-unobtrusive-bootstrap", filename: "jquery.validate.unobtrusive.bootstrap.js" },
        { src: "./node_modules/bootstrap/dist/js/bootstrap.js", dest: "./wwwroot/js/bootstrap" },
        { src: "./node_modules/jquery/dist/jquery.js", dest: "./wwwroot/js/jquery" },
        { src: "./node_modules/jquery-validation/dist/jquery.validate.js", dest: "./wwwroot/js/jquery-validation" },
        { src: "./node_modules/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js", dest: "./wwwroot/js/jquery-validation-unobtrusive" },
        { src: "./node_modules/popper.js/dist/umd/popper.js", dest: "./wwwroot/js/popper.js" },
        { src: "./Scripts/**/*.js", dest: "./wwwroot/js" }
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

// Css
gulp.task("copy-css", function (done) {
    _.forEach(app.css, function (file) {
        gulp.src(file)
            .pipe(sass())
            .pipe(gulp.dest("./wwwroot/css"));
    });

    done();
});

// Fonts
gulp.task("copy-fonts", function (done) {
    _.forEach(app.fonts, function (file) {
        gulp.src(file)
            .pipe(gulp.dest("./wwwroot/fonts"));
    });

    done();
});

// Images
gulp.task("copy-images", function (done) {
    _.forEach(app.images, function (file) {
        gulp.src(file)
            .pipe(gulp.dest("./wwwroot/images"));
    });

    done();
});

// JavaScript
gulp.task("copy-js", function (done) {
    _.forEach(app.js, function (file) {
        gulp.src(file.src)
            .pipe(gulpif(file.filename != null, rename(file.filename)))
            .pipe(gulp.dest(file.dest));
    });

    done();
});

gulp.task("default", gulp.series("copy-favicon", "copy-css", "copy-fonts", "copy-images", "copy-js"));