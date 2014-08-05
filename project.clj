(defproject potential-tribble "0.1.0-SNAPSHOT"
  :description "Small desktop application to convrt Wide World's website's team schedule to an importable calendar format"
  :url ""
  :license {:name "Eclipse Public License"
            :url "http://www.eclipse.org/legal/epl-v10.html"}
  :dependencies [[org.clojure/clojure "1.6.0"]
                 [enlive "1.1.5"]]
  :main ^:skip-aot potential-tribble.core
  :target-path "target/%s"
  :profiles {:uberjar {:aot :all}})
