![Datasilk Logo](http://www.markentingh.com/projects/datasilk/logo.png)

# Datasilk Core JavaScript
#### A simple, extensible JavaScript framework
Used alongside [Datasilk Core](http://www.github.com/Datasilk/Core), this JavaScript framework is meant to be used by web applications as a starting point. 

## Requirements
 Either [Selector](http://github.com/websilk/selector), jQuery, or any functional jQuery clone.

## Features
> NOTE: All features have been tested on various modern web browsers, including Internet Explorer 9 and up.

### S
`S` is a global JavaScript object that is used as a "Super" object, containing a hierarchy of methods & properties used within a web application.

### Ajax.js
Access RESTful web APIs and use `S.ajax.inject(data)` to load content into the DOM from the JSON response of a `Datasilk.Service.Response` web service object (found in [Datasilk Core](http://github.com/Datasilk/Core)). For example:

```
S.ajax.post('User/GetInfo', {userId:1, details:true, layout:3}, 
	function (d) {
		S.ajax.inject(d);
	}, 
	function (err) {
		S.message.show('.message', 'error', S.message.error.generic);
	}
);
```

### Loader.js
Display an SVG spinning loader animation on the page. For example:

```
$('body').html(S.loader({padding:5}));
```

### Message.js
Display a message on the page, such as an error or confirmation message above a form. For example:

```
<div class="message hide"><span></span></div>
```

```
S.message.show('.message', 'error', 'Incorrect password');
```

### Polyfill.js
Various polyfills for older web browsers, such as `Element.matches`, `Element.matchesSelector`, & `requestAnimationFrame` polyfills, 

### Popup.js
View a popup window above all the content on the web page. For example:

```
S.popup.show("New User", template_html, {offsetTop:-50, className:"new-user"});
```

### Scaffold.js
Load HTML content on the page while replacing `mustache` variables & blocks with dynamic data.

For example:

```
<script type="text/html" id="template_element">
    <div class="element">{{title}}</div>
    <div class="field"><input type="text" value="{{value}}"></div>
</script>

<script>
    var vars = {title: "Hello", value: "World"};
    var scaffold = new S.scaffold($('#template_element).val(), vars);
    $('body').append(scaffold.render());
</script>
```

### Util.js
Various utility functions, such as loading JavaScript & CSS files, injecting raw JavaScript code from a string, and injecting raw CSS styling from a string. For example:

```
S.util.css.add('bg_update', '.bg{background-color:#e0e0e0;}');
```

### Util.Color.js
Various color functions, such as converting RGB into HEX.

### Validate.js
Used for validating different kinds of data, such as an email address, credit card, or phone number

### Window.js
An accurate representation of the web browser window bounds & scroll positions.
