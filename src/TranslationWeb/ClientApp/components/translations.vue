<template>
    <div>
        <h2>Translations</h2>
        <p>Translate your all keys here and it will be stored directly.<br />If autotranslation is enabled you will get suggestions translated from main language</p>
        <span class="right"><strong>{{nokeys}}</strong> keys</span>

        <p v-if="!translations"><em><i class="fas fa-spinner fa-spin" />&nbsp;Loading</em></p>
        <ul class="langs">
            <li v-for="l in languages">
                {{l.name}}
            </li>
        </ul>
        <div class="addnew">
            <span>Add</span>
            <transinput isopen="true" locale="default" isnew="true" />
        </div>
        <table class="table" v-if="translations">
            <thead>
                <tr>
                    <th class="key">Key</th>
                    <th>Values</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(value,key) in translations.values">
                    <td class="key">
                        <div class="keyvalue">"{{ key }}"</div>
                        <div class="tools">
                            <span @click="dodelete(key)"><i class="fa fa-times" /></span>
                        </div>
                    </td>
                    <td class="valuetd">
                        <div class="value" v-for="(lang,idx) in value"><transinput :locale="translations.locales[idx]" :transkey="key" :value="lang" /></div>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
</template>
<script>
import Vue from 'vue'
import TransInput from './transinput'

Vue.component('transinput', TransInput);

export default {
    data() {
        return {
            translations: {}
        }
    },

    methods: {
        reload:function() {
            this.$http
                .get('/api/translation')
                .then(response => {
                    this.translations = response.data;
                });
        },
        dodelete:function(key) {
            this.$http.delete('/api/translation/'+key).then(response => {
                delete this.translations.values[key];
                this.reload();
            });
        }
    },
    computed: {
        nokeys: function() {
            if (this.translations.values)
                return Object.keys(this.translations.values).length;
            return 0;
        },
        languages: function(l) {

            if (this.translations.locales)
                return this.translations.locales.map(function(v,i) {
                    return {
                        locale: v,
                        idx: i,
                        name: window.$languageNames[v]
                    };
                });

            return [];
        }
    },
    async created() {
        try {
            console.log('http',this.$http);
            let response = await this.$http.get('/api/translation')
            console.log(response.data);
            this.translations = response.data;
        } catch (error) {
            console.log(error)
        }
    }
}
</script>
<style>
    .valuetd {
        background-color:#fcfcfc;
        border-bottom: 1px #aaa;
    }
    .right {
        float:right;
        font-size:20px;
    }
    .langs {
        list-style:none;
        margin:0;
        padding:0;
    }

    .keyvalue {
        text-align:right;
    }
    .tools {
        text-align:right;
    }
    .tools > span {
        margin-left: 10px;
        text-align: center;
        height:20px;
        width:20px;
        display:inline-block;
        line-height:20px;
        font-size: 12px;
        color: #fff;
        border-radius: 20px;
        background-color: red;
    }

    .langs li {
        display:inline-block;
        margin-right: 10px;
        padding: 2px 8px;
        text-transform: uppercase;
        color: #fff;
        border-radius: 20px;
        background-color: #53B7CE;
    }

    .value input {
        width: 190px;
        display:inline-block;
    }
</style>