S.util.color = {
    rgbToHex: function (color) {
        function toHex(num) {
            return ('0' + parseInt(num).toString(16)).slice(-2);
        }
        var hex = '';
        if (color.indexOf('rgb') >= 0) {
            var c = color.replace('rgb', '').replace('a', '').replace('(', '').replace(')', '').replace(/\s/g, '').split(',');
            hex = "#" + toHex(c[0]) + toHex(c[1]) + toHex(c[2]);
        } else { hex = color; }
        return hex;
    }
}