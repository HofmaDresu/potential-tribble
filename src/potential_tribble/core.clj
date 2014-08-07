(ns potential-tribble.core
  (:require [net.cgrand.enlive-html :as html])
  (:require [clojure.string :as cstr])
  (:gen-class))

(defn -main
  "I don't do a whole lot ... yet."
  [& args]
  (println "Hello, World!"))

(def wideworld-schedule-home-url "http://secure.hammerweb.net/wws_membership/SchedulesScoresDisplay.asp")

(defn get-page [url]
  (html/html-resource (java.net.URL. url)))

(defn get-wideworld-schedule-home []
  (get-page wideworld-schedule-home-url))

(def wideworld-schedule-home (get-wideworld-schedule-home))

(def seasons
  (let [potential-seasons (html/select wideworld-schedule-home [:p.style81])]
    ; Each potential season has a content mapping. Unfortunatly, there isn't a good way to differentiate between seasons
    ; and the Archived Standings links. Hence, the rest:
    (let [potential-season-content (map :content potential-seasons)]
      ; The only way that I've found to find the difference between seasons and the Archive standings is that 
      ; seasons have more first-level sub items than the link
      (let [season-strings (map last (filter #(>= (count %) 2) potential-season-content))]
        ; Seasons have a line break in them. I don't really want that hanging around, so split it out
        (map #(first (cstr/split % #"\n")) season-strings)))))
                                        
(defn get-league-table-selectors [season]
  (let [div-selectors (repeat (inc (.indexOf seasons season)) [:div (html/attr= :align "left")])]
    (flatten (conj (vec div-selectors) :table))))


(defn get-league-tables [season]
  (let [div-selectors (repeat (inc (.indexOf seasons season)) [:div (html/attr= :align "left")])]
    (let [full-selectors (flatten (conj (vec div-selectors) :table))]
      (let [league-tables (html/select wideworld-schedule-home (vec full-selectors))]
        league-tables))))

(defn get-leagues [season]
  (let [league-tables (get-league-tables season)]
    (let [league-list (html/select (first league-tables) [:span.style128])]
     (flatten (map :content league-list)))))




