/**
 * hpSupplies160x600 - An HTML banner
 * @version v1.0.0
 * @date 10-7-2016 at 17:17:32
 */
bannerInit = function() {
    function e(e) { t++, 2 == t && (t = 0, anim()) }

    function a(e) {}
    var t = 0,
        n = new Image;
    n.onload = e, n.onerror = a, n.src = "image.jpg";
    
    var o = new Image;
    o.onload = e, o.onerror = a, o.src = "printer.png", anim = function() {
        function e() {
            for (var e = 0; e < d; e++) {//d=30; so 30 particles
                for (var a = 0; a < y; a++) //y=1; so one time
                     this["myDotCont" + a] = new createjs.Container, 
                     this["hero" + a] = circ.clone(), 
                     this["hero" + a].theta = 360 * a / y, 
                     this["hero" + a].xbit = Math.sin(Math.PI / 180 * this["hero" + a].theta), 
                     this["hero" + a].ybit = Math.cos(Math.PI / 180 * this["hero" + a].theta), 
                     
                     this["myDotCont" + a].myXpos = Math.floor(this["hero" + a].xbit * c + 20 * Math.random() - 30), 
                     this["myDotCont" + a].myYpos = Math.floor(this["hero" + a].ybit * c + 200 * Math.random() - 200), 
                     
                     
                     this["myImage" + a] = myImage.clone(), 
                     this["myImage" + a].setTransform(-340 - this["myDotCont" + a].myXpos, -500 - this["myDotCont" + a].myYpos), 
                     
                     this["myDotCont" + a].addChild(this["myImage" + a]), 
                     
                     this["myImage" + a].mask = this["hero" + a], 
                     
                     this["myDotCont" + a].cache(-w, -w, 2 * w, 2 * w), 
                     this["myDotCont" + a].myYpos > -2000 && (p.push(C), 
                     
                     M.push(this["myDotCont" + a].myYpos), 
                     f.push(this["myDotCont" + a].myXpos), 
                     r1.addChild(this["myDotCont" + a]), 
                     
                     this["myDotCont" + a].y = -400, 
                     this["myDotCont" + a].scaleX = 2, 
                     this["myDotCont" + a].scaleY = 2, 
                     this["myDotCont" + a].alpha = 0, 
                     C++);
                     y += 6, 
                     c = c + 2 * w + 3, 
                     l++
            }
        }

        function a(e) {
            for (var a = e.length - 1; a >= 0; a--) {
                var t = Math.floor(Math.random() * a),
                    n = e[a];
                e[a] = e[t], e[t] = n
            }
            return e
        }

        function t(e) { 
            m.update(e) 
        }

        function r() { 
            TweenMax.to(container_ad, .5, { alpha: 1, ease: Cubic.easeOut }), 
            TweenMax.to(hd1, 1, { delay: .5, alpha: 1, ease: Power1.easeOut }), 
            TweenMax.delayedCall(2.75, s) 
        }

        function s() {
            createjs.Ticker.addEventListener("tick", t), 
            TweenMax.to(hd1, .5, { scaleX: 0, scaleY: 0, ease: Power1.easeOut }), 
            createjs.Ticker.addEventListener("tick", t);
            for (var e = 0; e < r1.getNumChildren(); e++) 
                TweenMax.to(r1.getChildAt(p[e]), 1, 
                    { delay: 7e-4 * e, alpha: 1, rotation: 0, x: f[p[e]], y: M[p[e]], 
                        scaleY: 1, 
                        scaleX: 1, 
                        ease: Power3.easeOut 
                    });
            
            TweenMax.to(myPrinter, 1.75, { delay: 3.25, x: -500, ease: Power1.easeIn }), 
            
            TweenMax.delayedCall(2.3, function() 
                { myMainCont.removeChild(r1) }), 
                  TweenMax.to(myMainCont, 5, { delay: 0, scaleX: .5, scaleY: .5, y: 600,x:170, ease: Power1.easeInOut, onComplete: function() { createjs.Ticker.removeEventListener("tick", t), TweenMax.delayedCall(1, i) } }), 
                  TweenMax.to(myWhite, 2, { delay: 1.5, alpha: 0, ease: Power1.easeIn })
             }

        function i() { 
            TweenMax.to(myCanvas, .45, { alpha: 0, ease: Power3.easeOut }), 
            TweenMax.to(hd2, .5, { alpha: 1, ease: Power1.easeOut }), 
            TweenMax.delayedCall(2, h) }

        function h() { 
            TweenMax.to(hd2, .5, { delay: .5, alpha: 0, ease: Power4.easeOut }), 
            TweenMax.to(product, .45, { delay: .5, alpha: 1, scaleX: 1, scaleY: 1, ease: Back.easeOut }), 
            TweenMax.to(hd3, 1, { delay: 1.25, alpha: 1, ease: Power1.easeOut }), 
			TweenMax.to(cta_id, 0.1, { delay: 1.25, left:29, ease: Power1.easeOut }), 
            TweenMax.to(logo, 1, { delay: 2.25, alpha: 1, ease: Power1.easeOut }), 
            TweenMax.to(tag, 1, { delay: 2.25, alpha: 1, ease: Power1.easeOut }) }
            TweenMax.set(product, { scaleX: 0, scaleY: 0 });

        var m;
        m = new createjs.Stage("myCanvas");
        var y = 1,
            c = 0,
            l = 0,
            d = 30,
            C = 0,
            w = 3;

        myMainCont = new createjs.Container, 
        myMainCont.setTransform(0, 0, 1.2, 1.2, 0, 0, 0, 160, 600), 
       
        myImage = new createjs.Bitmap(n), 
        myImage.setTransform(-140, 100), 
        myImage.cache(0, 0, 1200, 1e3), 
        
        myPrinter = new createjs.Bitmap(o), 
        //myPrinter.setTransform(-260, -550, 1.81,1.81), 
        myPrinter.setTransform(-285, -632, 1.93, 1.93),
       
        circ = new createjs.Shape, 
        circ.graphics.f("#fff").drawCircle(0, 0, w), 
        
        myWhite = new createjs.Shape, 
        myWhite.graphics.f("#fff").drawRect(-140, 100, 614, 930), 
        
        myArtCont = new createjs.Container, 
        myArtCont.setTransform(0, 0), 
       
        myMainCont.addChild(myArtCont), 
        r1 = new createjs.Container, 
        r1.setTransform(200, 600), 
        myArtCont.addChild(myImage, myWhite, r1);
        
        var M = [],
            f = [],
            p = [];
        e(), a(p), myMainCont.addChild(myPrinter), m.addChild(myMainCont);
        
        createjs.Ticker.setFPS(18);
        
        for (var u, T = 0; T < r1.getNumChildren(); T++) 
            u = T < 1450 ? -160 + .3 * T : 600 - .2 * (T - 1450), 
            r1.getChildAt(p[T]).x = -400, 
            r1.getChildAt(p[T]).rotation = .15 * u;
            r()
    }
};
