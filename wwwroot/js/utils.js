// Для вспомогательных функций
export function generate_guid() {
    var dt = new Date().getTime();
    return 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'.replace(/[x]/g, function (c) {
        var rnd = Math.random() * 16;
        rnd = (dt + rnd) % 16 | 0;
        dt = Math.floor(dt / 16);
        return (c === 'x' ? rnd : (rnd & 0x3 | 0x8)).toString(16);
    });
}