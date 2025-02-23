/* .vitepress/theme/index.ts */
import DefaultTheme from 'vitepress/theme'
import './style/index.css'
import Mycomponent from "./components/Mycomponent.vue"
import Linkcard from "./components/Linkcard.vue"
import xgplayer from "./components/xgplayer.vue"
import DataPanel from "./components/DataPanel.vue"
import update from "./components/update.vue"
import MyLayout from './components/MyLayout.vue'

import { inBrowser } from 'vitepress'
import busuanzi from 'busuanzi.pure.js'

export default {
  extends: DefaultTheme,
  Layout: MyLayout,
  enhanceApp({app , router}) { 
    if (inBrowser) {
      router.onAfterRouteChanged = () => {
        busuanzi.fetch()
      }
    }
    // 注册全局组件
    app.component('Mycomponent' , Mycomponent)
    app.component('Linkcard' , Linkcard)
    app.component('xgplayer' , xgplayer)
    app.component('DataPanel' , DataPanel)
    app.component('update' , update)
  },
}