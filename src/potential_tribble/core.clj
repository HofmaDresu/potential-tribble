(ns potential-tribble.core
  (:require [clj-http.client :as http])
  (:gen-class))

(defn -main
  "I don't do a whole lot ... yet."
  [& args]
  (println "Hello, World!"))

(def wideworld-schedule-home "http://secure.hammerweb.net/wws_membership/SchedulesScoresDisplay.asp")

(defn get-page-body [url]
  (:body (http/get url)))

(defn get-wideworld-schedule-home []
  (get-page-body wideworld-schedule-home))

(def wideworld-schedule-home-body (get-wideworld-schedule-home))
