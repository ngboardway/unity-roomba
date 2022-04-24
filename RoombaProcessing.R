library(readr)
library(dplyr)
library(ggplot2)
library(ggpubr)

results <- read_csv("roomba-results.csv")
head(results)

# Means for metrics x pathing type
pathing_type_means <- results %>%
  group_by(pathing_type) %>%
  summarize(battery = mean(battery_remaining),
            dirt = mean(percent_dirt_collected),
            board = mean(percent_board_covered),
            time = mean(time_taken))
pathing_type_means

# Means for metrics x rooom layout type
room_type_means <- results %>%
  group_by(room_layout_type) %>%
  summarize(batteryx = mean(battery_remaining),
            dirt = mean(percent_dirt_collected),
            board = mean(percent_board_covered),
            time = mean(time_taken))
room_type_means

# P values for metrics x pathing type (using Mann-Whitney U Tes)
pathing_battery <- wilcox.test(battery_remaining~ pathing_type, data = results)
pathing_dirt <- wilcox.test(percent_dirt_collected~ pathing_type, data = results)
pathing_time <- wilcox.test(time_taken~ pathing_type, data = results)
pathing_board <- wilcox.test(percent_board_covered~ pathing_type, data = results)

pathing_battery
pathing_dirt
pathing_time
pathing_board

# P values for metrics x room layout type (using Kruskal–Wallis Test)
room_battery <- kruskal.test(battery_remaining~ room_layout_type, data = results)
room_dirt <- kruskal.test(percent_dirt_collected~ room_layout_type, data = results)
room_time <- kruskal.test(time_taken~ room_layout_type, data = results)
room_board <- kruskal.test(percent_board_covered~ room_layout_type, data = results)

room_battery
room_dirt
room_time
room_board

# Box plots for metrics
pb<-ggplot(results, aes(x=room_layout_type, y = percent_board_covered, fill=pathing_type)) +
  geom_boxplot() +
  xlab("Room Layout Type") +
  ylab("Percent") +
  guides(fill = guide_legend(title = "Pathing Type")) +
  labs(title = "Percent of Board Covered") +
  scale_y_continuous(breaks=seq(0,100,20))  +
  scale_x_discrete(limits = c('Square', 'SingleRoom', 'Apartment'))
pb

pd<-ggplot(results, aes(x=room_layout_type, y = percent_dirt_collected, fill=pathing_type)) +
  geom_boxplot() +
  xlab("Room Layout Type") +
  ylab("Percent") +
  guides(fill = guide_legend(title = "Pathing Type")) +
  labs(title = "Percent of Dirt Collected") +
  scale_y_continuous(breaks=seq(0,100,20)) +
  scale_x_discrete(limits = c('Square', 'SingleRoom', 'Apartment'))
pd

pr<-ggplot(results, aes(x=room_layout_type, y = battery_remaining, fill=pathing_type)) +
  geom_boxplot() +
  xlab("Room Layout Type") +
  ylab("Percent") +
  guides(fill = guide_legend(title = "Battery Remaining")) +
  labs(title = "Percent of Dirt Collected") +
  scale_y_continuous(breaks=seq(0,100,20)) +
  scale_x_discrete(limits = c('Square', 'SingleRoom', 'Apartment'))
pr

pt<-ggplot(results, aes(x=room_layout_type, y = time_taken, fill=pathing_type)) +
  geom_boxplot() +
  xlab("Room Layout Type") +
  ylab("Timesteps") +
  guides(fill = guide_legend(title = "Pathing Type")) +
  labs(title = "Time Taken") +
  scale_x_discrete(limits = c('Square', 'SingleRoom', 'Apartment'))
pt
